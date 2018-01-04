namespace VisualPlus.Localization
{
    public sealed class PropertyCategory
    {
#if DEBUG
        public const string Accessibility = GlobalStrings.DefaultCategoryText;
        public const string Appearance = GlobalStrings.DefaultCategoryText;
        public const string Behavior = GlobalStrings.DefaultCategoryText;
        public const string Data = GlobalStrings.DefaultCategoryText;
        public const string Design = GlobalStrings.DefaultCategoryText;
        public const string Focus = GlobalStrings.DefaultCategoryText;
        public const string Layout = GlobalStrings.DefaultCategoryText;
        public const string WindowStyle = GlobalStrings.DefaultCategoryText;

#else
            public const string Accessibility = "Accessibility";
            public const string Appearance = "Appearance";
            public const string Behavior = "Behavior";
            public const string Layout = "Layout";
            public const string Data = "Data";
            public const string Design = "Design";
            public const string Focus = "Focus";
            public const string WindowStyle = "Window style";
#endif
    }
}