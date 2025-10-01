using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class SlaytApiRequest : ISlaytApiRequest
	{
		private readonly IRequestService _requestService;
		public SlaytApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}
		public SlaytlarDto GetSlaytByEmailPassword(string email, string password)
		{
			return _requestService.Get<SlaytlarDto>("Slayt/GetSlaytByEmailPassword?email=" + email + "&password=" + password);
		}
		public bool DeleteSlayt(int slaytId)
		{
			return _requestService.Get<bool>("Slayt/DeleteSlayt?slaytId=" + slaytId);
		}

		public List<SlaytlarDto> GetAllSlayt()
		{
			return _requestService.Get<List<SlaytlarDto>>("Slayt/GetAllSlayt");
		}

		public SlaytlarDto GetSlaytById(int slaytId) => _requestService.Get<SlaytlarDto>("Slayt/GetSlaytById?slaytId=" + slaytId);


		public SlaytlarDto InsertSlayt(SlaytlarDto model)
		{
			return _requestService.Post<SlaytlarDto>("Slayt/InsertSlayt", model);
		}

		public SlaytlarDto UpdateSlayt(SlaytlarDto model)
		{
			return _requestService.Post<SlaytlarDto>("Slayt/UpdateSlayt", model);
		}
	}
}
