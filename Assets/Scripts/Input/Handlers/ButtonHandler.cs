namespace RPG
{
    public class ButtonHandler : DerivedHandler<float, bool>
    {
        public ButtonHandler() : base(
            from: new ValueHandler<float>(),
            derive: (value) => value != default
        )
        { }
    }
}
