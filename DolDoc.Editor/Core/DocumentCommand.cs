namespace DolDoc.Editor.Core
{
    /// <summary>
    /// Contains all possible DolDoc commands.
    /// </summary>
    public enum DocumentCommand : byte
    {
        /// <summary>
        /// Error command
        /// </summary>
        Error,

        /// <summary>
        /// Set the background color to a value, or reset it to its default value.
        /// <code>$BG,RED$</code>
        /// or
        /// <code>$BG$</code>
        /// </summary>
        Background,

        /// <summary>
        /// Enables or disables blinking mode.
        /// <code>$BK,1$</code>
        /// </summary>
        Blink,

        /// <summary>
        /// Clear all entries before this entry, skipping those that have the Hold (+H) flag.
        /// <code>$CL$</code>
        /// </summary>
        Clear,

        /// <summary>
        /// Set the foreground color to a value, or reset it to its default value.
        /// <code>$FG,RED$</code>
        /// or
        /// <code>$FG$</code>
        /// </summary>
        Foreground,

        /// <summary>
        /// Sets the current indentation level. Can be negative.
        /// <code>$ID,-2$</code>
        /// </summary>
        Indent,

        /// <summary>
        /// Enables or disables inverted rendering mode.
        /// <code>$IV,1$</code>
        /// </summary>
        Invert,

        /// <summary>
        /// Renders a link to the screen.
        /// <code>$LK,"My text",A="location"$</code>
        /// </summary>
        Link,

        /// <summary>
        /// Moves the cursor relative to its current position, or a reference point set by a flag.
        /// <code>$CM,4,10$</code>
        /// or
        /// <code>$CM+LX,10,-4$</code>
        /// </summary>
        MoveCursor,

        /// <summary>
        /// Write a string of characters to the screen.
        /// <code>$TX,"Hello world!"$</code>
        /// </summary>
        Text,

        /// <summary>
        /// ???
        /// </summary>
        Tree,

        /// <summary>
        /// Enables or disables underline mode.
        /// <code>$UL,1$ / $UL,0$</code>
        /// </summary>
        Underline,
        
        /// <summary>
        /// Enables or disables word-wrap mode.
        /// <code>$WW,1$ / $WW,0$</code>
        /// </summary>
        WordWrap
    }
}
