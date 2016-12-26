namespace Xemio.Apps.Windows.Services.Queries
{
    public class QueryResult<TResult>
    {
        public QueryResult(bool isStale, TResult result)
        {
            this.IsStale = isStale;
            this.Result = result;
        }

        public bool IsStale { get; }
        public TResult Result { get; }
    }
}