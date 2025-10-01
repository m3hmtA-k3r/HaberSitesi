using Microsoft.AspNetCore.Http;

namespace ApiAccess.Abstract
{
	public interface ICommonApiRequest
	{
		string Upload(IFormFile file);
	}
}
