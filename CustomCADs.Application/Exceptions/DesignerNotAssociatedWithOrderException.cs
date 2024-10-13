namespace CustomCADs.Application.Exceptions
{
    [Serializable]
    public class DesignerNotAssociatedWithOrderException : Exception
    {
        public DesignerNotAssociatedWithOrderException(): base("The requested Order is not associated with the provided Designer.") { }
        public DesignerNotAssociatedWithOrderException(int orderId, string designerName): base($"The Order with id: {orderId} is not associated with the Designer: {designerName}.") { }
        public DesignerNotAssociatedWithOrderException(string message): base(message) { }
        public DesignerNotAssociatedWithOrderException(string message, Exception inner): base(message, inner) { }
    }
}
