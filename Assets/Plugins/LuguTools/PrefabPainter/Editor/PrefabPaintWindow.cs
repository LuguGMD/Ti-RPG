using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PrefabPaintWindow : EditorWindow
{
    [SerializeField] private GameObject _groupObject;

    [SerializeField] private float _brushSize = 1.0f;
    [SerializeField] private float _brushDensity = 1.0f;
    [SerializeField] private float _brushSpacing = 0.5f;

    [SerializeField] private GameObject _prefabToPaint;

    [SerializeField] private int _layerMask = 0;

    [SerializeField] private float _rotationVariance = 30f;
    [SerializeField] private float _scaleVariance = 0.1f;

    [SerializeField] private float _angleTreshold = 90f;

    private bool _canPaint = true;
    private Vector3 _lastPaintPosition = Vector3.zero;
    private Vector3 _lastPaintNormal = Vector3.zero;

    private enum State
    {
        Paint,
        Erase
    }

    [SerializeField] private State _currentState = State.Paint;

    private SerializedObject _paintWindowSO;

    private SerializedProperty _groupObjectProp;

    private SerializedProperty _brushSizeProp;
    private SerializedProperty _brushDensityProp;
    private SerializedProperty _brushSpacingProp;
    private SerializedProperty _prefabToPaintProp;
    private SerializedProperty _layerMaskProp;

    private SerializedProperty _rotationVarianceProp;
    private SerializedProperty _scaleVarianceProp;

    private SerializedProperty _angleTresholdProp;

    private const float RAYCAST_HEIGHT_OFFSET = 3f;
    private const float RAYCAST_MAX_DISTANCE = 10f;

    void OnEnable()
    {
        SceneView.duringSceneGui += this.OnSceneGUI;

        _paintWindowSO = new SerializedObject(this);

        _groupObjectProp = _paintWindowSO.FindProperty("_groupObject");

        _brushSizeProp = _paintWindowSO.FindProperty("_brushSize");
        _brushDensityProp = _paintWindowSO.FindProperty("_brushDensity");
        _brushSpacingProp = _paintWindowSO.FindProperty("_brushSpacing");

        _prefabToPaintProp = _paintWindowSO.FindProperty("_prefabToPaint");
        _layerMaskProp = _paintWindowSO.FindProperty("_layerMask");

        _rotationVarianceProp = _paintWindowSO.FindProperty("_rotationVariance");
        _scaleVarianceProp = _paintWindowSO.FindProperty("_scaleVariance");

        _angleTresholdProp = _paintWindowSO.FindProperty("_angleTreshold");
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    [MenuItem("Tools/Lugu/Prefab Paint #&P")]
    public static void OpenWindow()
    {
        PrefabPaintWindow window = GetWindow<PrefabPaintWindow>("Prefab Paint Tool");
        window.minSize = new Vector2(450, 200);
        window.Show();
    }

    private void OnGUI()
    {
        _currentState = (State)GUILayout.Toolbar((int)_currentState, System.Enum.GetNames(typeof(State)));

        _paintWindowSO.Update();

        string _prefabFieldName = _currentState == State.Paint ? "Prefab to Paint" : "Prefab to Erase";
        _prefabToPaintProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField(_prefabFieldName, _prefabToPaint, typeof(GameObject), false);

        Rect rect = GUILayoutUtility.GetRect(0, 20);
        _layerMaskProp.intValue = EditorGUI.LayerField(rect, "Layer Mask", _layerMask);

        if (_currentState == State.Paint)
        {
            _groupObjectProp.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Parent Object", _groupObject, typeof(GameObject), true);
            _brushSizeProp.floatValue = EditorGUILayout.FloatField("Brush Size", _brushSize);
            _brushDensityProp.floatValue = EditorGUILayout.Slider("Brush Density", _brushDensity, 0, 1);
            _brushSpacingProp.floatValue = EditorGUILayout.Slider("Brush Spacing", _brushSpacing, 0, 1);

            _rotationVarianceProp.floatValue = EditorGUILayout.FloatField("Rotation Variance", _rotationVariance);
            _scaleVarianceProp.floatValue = EditorGUILayout.FloatField("Scale Variance", _scaleVariance);
            _angleTresholdProp.floatValue = EditorGUILayout.FloatField("Angle Treshold", _angleTreshold);
        }

        _paintWindowSO.ApplyModifiedProperties();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        sceneView.Repaint();

        //Checking if Raycast hits something
        if (RaycastFromMouse(out Vector3 point, out Vector3 normal))
        {

            //Checking for paint input
            if (CheckPaintInput())
            {
                switch (_currentState)
                {
                    case State.Paint:
                        HandlePaint(point, normal);
                        break;
                    case State.Erase:
                        HandleErase(point, normal);
                        return;
                }

            }
        }

        if (_canPaint == false)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                _canPaint = true;
            }
            else if (Vector3.Distance(_lastPaintPosition, point) > _brushSpacing * _brushSize)
            {
                _canPaint = true;
            }
        }

        Event e = Event.current;
        
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            GoToNextState();
        }
    }

    private void HandlePaint(Vector3 point, Vector3 normal)
    {
        _canPaint = false;
        Undo.SetCurrentGroupName("Paint Prefab Group");
        int group = Undo.GetCurrentGroup();
        PlaceAllPrefabs(point, normal);

        Undo.CollapseUndoOperations(group);
    }

    private void HandleErase(Vector3 point, Vector3 normal)
    {
        Undo.SetCurrentGroupName("Erase Prefab Group");
        int group = Undo.GetCurrentGroup();

        Collider[] _hits = Physics.OverlapSphere(point, _brushSize, _layerMask - 1);

        if (_hits.Length > 0)
        {
            foreach (Collider hit in _hits)
            {
                if (PrefabUtility.GetPrefabInstanceHandle(hit.gameObject) == null)
                    continue;

                GameObject hitObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(hit.gameObject);

                if (hitObject == _prefabToPaint)
                {
                    Undo.DestroyObjectImmediate(hit.gameObject);
                }
            }
        }

        Undo.CollapseUndoOperations(group);
    }

    private bool RaycastFromMouse(out Vector3 point, out Vector3 normal)
    {
        point = Vector3.zero;
        normal = Vector3.zero;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (!_canPaint)
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.color = new UnityEngine.Color(1f, 1f, 1f, 0.35f);
            Handles.DrawWireDisc(_lastPaintPosition, _lastPaintNormal, _brushSpacing * _brushSize);
            Handles.color = UnityEngine.Color.white;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask - 1))
        {
            point = hit.point;
            normal = hit.normal;

            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawWireDisc(hit.point, hit.normal, _brushSize);
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

            return true;
        }

        return false;
    }

    private bool CheckPaintInput()
    {
        Event e = Event.current;
        if (_canPaint)
        {
            if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0 && !e.alt)
            {
                e.Use();
                return true;
            }
        }

        return false;
    }

    private void PlaceAllPrefabs(Vector3 point, Vector3 normal)
    {
        if (_groupObject == null)
        {
            CreateGroupObject();
        }

        _lastPaintPosition = point;
        _lastPaintNormal = normal;

        float area = Mathf.PI * _brushSize * _brushSize;
        int prefabCount = Mathf.RoundToInt(area * _brushDensity);

        for (int i = 0; i < prefabCount; i++)
        {
            Vector3 randompos = GetRandomPointInCircle(point, normal);
            GetFixedPosition(randompos, normal);
        }
    }

    private void PlacePrefabAtPoint(Vector3 point, Vector3 normal)
    {
        if (_prefabToPaint)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(_prefabToPaint);
            newObj.transform.position = point;
            newObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);

            float randomYRotation = Random.Range(-_rotationVariance, _rotationVariance);
            newObj.transform.Rotate(Vector3.up, randomYRotation, Space.Self);

            float randomScaleFactor = 1 + Random.Range(-_scaleVariance, _scaleVariance);
            newObj.transform.localScale = newObj.transform.localScale * randomScaleFactor;

            newObj.transform.parent = _groupObject.transform;

            Undo.RegisterCreatedObjectUndo(newObj, "Paint Prefab");
        }
    }

    private Vector3 GetRandomPointInCircle(Vector3 point, Vector3 normal)
    {
        Vector2 randomPoint = Random.insideUnitCircle * _brushSize;

        Vector3 tangent = Vector3.Cross(normal, Vector3.up).normalized;

        if (tangent == Vector3.zero)
        {
            tangent = Vector3.Cross(normal, Vector3.right).normalized;
        }

        Vector3 bitangent = Vector3.Cross(normal, tangent).normalized;

        Vector3 offset = tangent * randomPoint.x + bitangent * randomPoint.y;
        point += offset;

        return point;
    }

    private void GetFixedPosition(Vector3 point, Vector3 normal)
    {
        Ray ray = new Ray(point + normal * RAYCAST_HEIGHT_OFFSET, -normal);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_MAX_DISTANCE, _layerMask - 1))
        {
            if (Vector3.Angle(Vector3.up, hit.normal) < _angleTreshold)
            {
                PlacePrefabAtPoint(hit.point, hit.normal);
            }
        }
    }

    private void CreateGroupObject()
    {
        _paintWindowSO.Update();
        GameObject group = new GameObject("PaintedPrefabs_Group");
        _groupObjectProp.objectReferenceValue = group;
        _paintWindowSO.ApplyModifiedProperties();
        Undo.RegisterCreatedObjectUndo(group, "Paint Group Object");
    }

    private void GoToNextState()
    {
        _currentState = (State)(((int)_currentState + 1) % System.Enum.GetNames(typeof(State)).Length);
        Repaint();
    }
}
