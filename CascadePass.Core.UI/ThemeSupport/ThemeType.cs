namespace CascadePass.Core.UI
{
    /// <summary>
    /// Represents a type or category of theme, of which there can be many individual
    /// themes.  Applications typically implement their own themes, sometimes many;
    /// ThemeType describes the categories these should fall into.
    /// </summary>
    public enum ThemeType
    {
        /// <summary>
        /// Value has not ben set.
        /// </summary>
        None,

        /// <summary>
        /// Light mode.
        /// </summary>
        Light,
 
        /// <summary>
        /// Dark mode.
        /// </summary>
        Dark,
        
        /// <summary>
        /// High contrast mode, often used with other accessibility features.
        /// </summary>
        HighContrast
    }
}
