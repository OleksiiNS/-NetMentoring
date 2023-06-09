﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "Equals")
            {
                var predicate = Expression.Equal(node.Object, Expression.Constant($"{node.Arguments[0].ToString().Trim('"')}"));
                Visit(predicate);

                return node;
            }
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "Contains")
            {
                var predicate = Expression.Equal(node.Object, Expression.Constant($"*{node.Arguments[0].ToString().Trim('"')}*"));
                Visit(predicate);

                return node;
            }
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "StartsWith")
            {
                var predicate = Expression.Equal(node.Object, Expression.Constant($"{node.Arguments[0].ToString().Trim('"')}*"));
                Visit(predicate);

                return node;
            }
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "EndsWith")
            {
                var predicate = Expression.Equal(node.Object, Expression.Constant($"*{node.Arguments[0].ToString().Trim('"')}"));
                Visit(predicate);

                return node;
            }
            return base.VisitMethodCall(node);
        }


        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType == ExpressionType.MemberAccess && node.Right.NodeType == ExpressionType.Constant)
                    {
                        Visit(node.Left);
                        _resultStringBuilder.Append('(');
                        Visit(node.Right);
                        _resultStringBuilder.Append(')');
                    }
                    else if(node.Right.NodeType == ExpressionType.MemberAccess && node.Left.NodeType == ExpressionType.Constant)
                    {
                        Visit(node.Right);
                        _resultStringBuilder.Append('(');
                        Visit(node.Left);
                        _resultStringBuilder.Append(')');
                    }
                    else
                    {
                        throw new NotSupportedException($"One operand should be property or field and another operand should be constant");
                    }                    
                    break;
                case ExpressionType.AndAlso:
                    _resultStringBuilder.Append("\"statements\": [");
                    AddStatement(node.Left);
                    _resultStringBuilder.Append(",");
                    AddStatement(node.Right);
                    _resultStringBuilder.Append("\r\n]");
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        private void AddStatement(Expression node)
        {
            _resultStringBuilder.Append("\r\n{\"query\":\"");
            Visit(node);
            _resultStringBuilder.Append("\"}");
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
