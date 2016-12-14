using System;
using CQRS;
using CQRS.DAL;

namespace TimeService
{
    public class GetTimeQuery : IQuery
    {
        public string ServiceName => "timeService";
    }

    public class GetTimeQueryHandler : IQueryHandler<GetTimeQuery, string>
    {
        public string Handle(GetTimeQuery query)
        {
            return DateTime.Now.ToString();
        }
    }
}