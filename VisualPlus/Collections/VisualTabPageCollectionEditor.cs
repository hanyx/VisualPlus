namespace VisualPlus.Collections
{
    #region Namespace

    using System;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    using VisualPlus.Toolkit.Child;

    #endregion

    internal class VisualTabPageCollectionEditor : CollectionEditor
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="VisualTabPageCollectionEditor" /> class.</summary>
        /// <param name="type">The type.</param>
        public VisualTabPageCollectionEditor(Type type) : base(type)
        {
        }

        #endregion

        #region Events

        /// <summary>Creates a new item types.</summary>
        /// <returns>
        ///     <see cref="Type" />
        /// </returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new[] { typeof(TabPage), typeof(VisualTabPage) };
        }

        #endregion
    }
}