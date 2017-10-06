namespace VisualPlus.Localization.Category
{
    public sealed class Events
    {
#if DEBUG
        public const string Action = GlobalStrings.DefaultCategoryText;
        public const string Appearance = GlobalStrings.DefaultCategoryText;
        public const string Behavior = GlobalStrings.DefaultCategoryText;
        public const string Data = GlobalStrings.DefaultCategoryText;
        public const string DragDrop = GlobalStrings.DefaultCategoryText;
        public const string Focus = GlobalStrings.DefaultCategoryText;
        public const string Key = GlobalStrings.DefaultCategoryText;
        public const string Layout = GlobalStrings.DefaultCategoryText;
        public const string Misc = GlobalStrings.DefaultCategoryText;
        public const string Mouse = GlobalStrings.DefaultCategoryText;
        public const string PropertyChanged = GlobalStrings.DefaultCategoryText;
#else
            public const string Action = "Action";
            public const string Appearance = "Appearance";
            public const string Behavior = "Behavior";
            public const string Data = "Data";
            public const string DragDrop = "Drag Drop";
            public const string Focus = "Focus";
            public const string Key = "Key";
            public const string Layout = "Layout";
            public const string Misc = "Misc";
            public const string Mouse = "Mouse";
            public const string PropertyChanged = "Property Changed";
#endif
    }
}