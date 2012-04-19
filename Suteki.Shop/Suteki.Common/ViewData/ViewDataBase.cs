namespace Suteki.Common.ViewData
{
    public abstract class ViewDataBase : IMessageViewData, IErrorViewData
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }

        public ViewDataBase WithErrorMessage(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            return this;
        }

        public ViewDataBase WithMessage(string message)
        {
            this.Message = message;
            return this;
        }
    }
}
