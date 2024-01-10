namespace Space.Infrastructure.Helpers;

internal class ExpressionHelper : ExpressionVisitor
{
    private readonly Dictionary<string, bool> _memberExpressions;
    public ExpressionHelper(params string[] members)
    {
        _memberExpressions = new Dictionary<string, bool>();
        foreach (string item in members)
        {
            _memberExpressions.Add(item, false);
        }
    }

    public Dictionary<string, bool> ContainsMembers(Expression expression)
    {
        Visit(expression);
        return _memberExpressions;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (_memberExpressions.GetValueOrDefault(node.Member.Name))
        {
            _memberExpressions[node.Member.Name] = true;
        }
        return base.VisitMember(node);
    }
}
