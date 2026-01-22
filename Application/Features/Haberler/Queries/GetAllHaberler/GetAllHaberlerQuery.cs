using Application.DTOs;
using MediatR;

namespace Application.Features.Haberler.Queries.GetAllHaberler
{
	/// <summary>
	/// Query to get all Haberler (News Articles)
	/// Uses CQRS pattern - Queries are read-only operations
	/// </summary>
	public class GetAllHaberlerQuery : IRequest<List<HaberlerDto>>
	{
		// Empty query - gets all news
	}
}
