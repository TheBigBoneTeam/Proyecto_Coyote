using System;

namespace BehaviourAPI.BehaviourTrees
{
    public class TrueFalseBranchNode : BranchNode, ITrueFalseBranchNode
    {
        /// <summary>
        /// The function used to get the branch index. The result will be clamped between 0 and child count.
        /// </summary>
        public Func<bool> nodeIndexFunction;

        /// <summary>
        /// Set the function used to get the branch index.
        /// </summary>
        /// <param name="nodeIndexFunction">The value of the function.</param>
        /// <returns>The <see cref="FunctionBranchNode"/> itself.</returns>
        public TrueFalseBranchNode SetNodeIndexFunction(Func<bool> nodeIndexFunction)
        {
            this.nodeIndexFunction = nodeIndexFunction;
            return this;
        }

        protected override int SelectBranchIndex()
        {
            bool index = nodeIndexFunction?.Invoke() ?? false;
            return index ? 1 : 0;
        }
    }
}
