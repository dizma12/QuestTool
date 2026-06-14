namespace QuestMaker.Domain
{
    public enum QuestType
    {

        /// <summary>
        /// Hidden (-1) is not depricated, its marked obsolete so it wont show in the inspector
        /// </summary>
        [System.Obsolete("Hidden is not depricated, its marked obsolete so it wont show in the inspector", false)] // Mark obsolete so it wont show up in inspector.
        Hidden = -1,

        Main,
        Side,
        Repeatable,
        Timed,
        Daily,
        Unlockable,
    }
}
