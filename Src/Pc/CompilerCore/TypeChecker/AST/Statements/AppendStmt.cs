﻿using Antlr4.Runtime;

namespace Plang.Compiler.TypeChecker.AST.Statements
{
    class AppendStmt : IPStmt
    {
        public AppendStmt(ParserRuleContext sourceLocation, IPExpr array, IPExpr value)
        {
            SourceLocation = sourceLocation;
            Array = array;
            Value = value;
        }

        public ParserRuleContext SourceLocation { get; }
        public IPExpr Array { get; }
        public IPExpr Value { get; }
    }
}
