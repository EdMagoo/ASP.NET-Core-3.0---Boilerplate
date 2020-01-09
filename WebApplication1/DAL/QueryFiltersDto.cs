using System;

namespace WebApplication1.DAL
{
    /* the client must have this enum to use the custom query API */
    /* where ops */
    [Flags]
    public enum WhereOperation
    {
        EqualTo,
        LessThan,
        GreaterThan,
        LessThanOrEquals,
        GreaterThanOrEquals,
        NotEqualTo,
        Like,
        IsNull,
        NotLike,
        NotNull,
        Between,
        In,
        Some
    }

    [Flags]
    public enum WhereOperator
    {
        And,
        Or,
        Not
    }
    
    /* sort by ops */
    public enum SortOperation
    {
        Desc,
        Asc
    }

    /*
     * Example 1: Where property1 == value1 <- no chain
     * Example 2: Where property1 == value1 AND property2 != value2 <- with chainOp
     * Example 2.1: Where property1 Between value1 AND value2 <- with chainOp
     *
     * How to use it:
     * implement a recursive method to get the next filter
     */
    public class QueryWhereFilter
    {
        // #1
        public WhereOperation? WhereOperation { get; set; }
        // #2
        public string Property { get; set; }
        // #3 <- it is possible that this would be another property or a function to compare. in future replace for something that match a value and a property
        public object Value { get; set; }
        // #4 note for use: the first QueryWhereFilter doesn't have a WhereOperator
        public WhereOperator? ChainOperator { get; set; }
        // #5
        public QueryWhereFilter NextFilter { get; set; }
    }
    
    public class QueryFiltersDto
    {
        public QueryWhereFilter QueryWhereFilter { get; set; }
        // Sort operations
    }
}