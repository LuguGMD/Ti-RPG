using static UnityEngine.InputSystem.InputAction;

namespace RPG
{

    public class ValueHandler<TValue> : InputHandler<TValue>
        where TValue : struct
    {
        public override TValue GetValue(CallbackContext context)
        {
            TValue value = context.ReadValue<TValue>();
            return value;
        }
    }
}
