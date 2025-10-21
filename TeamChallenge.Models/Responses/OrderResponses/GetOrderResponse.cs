using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamChallenge.Models.Responses.OrderResponses
{
    public class GetOrderResponse : BaseDataResponse<GetOrderResponseModel>
    {
        public GetOrderResponse(GetOrderResponseModel data) : base(data)
        {
            
        }
    }
}
