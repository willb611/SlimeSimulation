namespace SlimeSimulation.Model.Generation
{
    public class EdgeConnectionShape
    {
        public const int EdgeConnectionShapeSquare = 1;
        public const string EdgeConnectionShapeSquareDescription = "Square";

        public const int EdgeConnectionShapeSquareWithDiamonds = 2;
        public const string EdgeConnectionShapeSquareWithDiamondsDescription = "Square with larger diamonds";

        public const int EdgeConnectionShapeSquareWithCrossedDiagonals = 3;
        public const string EdgeConnectionShapeSquareWithCrossedDiagonalsDescription = "Square with crossing diagonals";

        public static int DefaultEdgeConnectionType => EdgeConnectionShapeSquare;

        public static string[] DescriptionsForEdgeConnectionTypes = new string[]
        {
            EdgeConnectionShapeSquareDescription, EdgeConnectionShapeSquareWithDiamondsDescription,
            EdgeConnectionShapeSquareWithCrossedDiagonalsDescription
        };

        public static int GetValueForDescription(string descriptionOfShapeFormedByEdges)
        {
            switch (descriptionOfShapeFormedByEdges)
            {
                default:
                case EdgeConnectionShapeSquareDescription:
                    return EdgeConnectionShapeSquare;
                case EdgeConnectionShapeSquareWithCrossedDiagonalsDescription:
                    return EdgeConnectionShapeSquareWithCrossedDiagonals;
                case EdgeConnectionShapeSquareWithDiamondsDescription:
                    return EdgeConnectionShapeSquareWithDiamonds;
            }
        }

        public static int IndexInDescriptionArrayForValue(int edgeConnectionType)
        {
            if (edgeConnectionType <= 3 && edgeConnectionType >= 1)
            {
                return edgeConnectionType - 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
