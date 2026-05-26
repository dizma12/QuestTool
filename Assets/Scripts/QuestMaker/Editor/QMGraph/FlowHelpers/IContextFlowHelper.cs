namespace QuestMaker.Editor.Nodes
{

    internal interface IGraphFlowHelper { };

    /// <summary>
    /// Helper Interface for all non objective nodes;
    /// (For Objective nodes use IObjectiveFlowHelper)
    /// </summary>
    internal interface IContextFlowHelper : IGraphFlowHelper{ };

    /// <summary>
    ///Helper interface for all Objective context nodes
    ///(For normal nodes use IGraphFlowHelper)
    /// </summary>
    internal interface IObjectiveFlowHelper : IGraphFlowHelper { };

    internal interface ISpecialEventNode { };
}
 