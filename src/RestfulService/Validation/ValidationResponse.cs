namespace RestfulService.Validation
{
	public class ValidationResponse
	{
		public int ErrorCode{get; set; }
		public string ErrorMessage { get; set; }

		public ValidationResponse(int errorCode, string errorMessage) {
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}
	}
}