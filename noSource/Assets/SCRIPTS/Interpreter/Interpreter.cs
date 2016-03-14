using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public enum Attr
    {
        BOOL, CHAR, FLOAT, INT, STRING, LVAL, CONST, SIZE
    }
    public class ASTree
    {
        public Tokens Token { get; set; }
        public int Linenr { get; set; }
        public int Offset { get; set; }
        public int Blocknr { get; set; }
        public string LexInfo { get; set; }
        public Symbol Symbol { get; set; }
        public List<ASTree> Children { get; set; }
        public BitArray Attributes { get; set; }
        private string _value = "";
        public string Value { get { return _value; } set { _value = value; if (Symbol != null) Symbol.Value = value; } }
        
        public ASTree(Tokens token, int linenr, int offset, string lexinfo)
        {
            Token = token;
            Linenr = linenr;
            Offset = offset;
            LexInfo = lexinfo;
            Children = new List<ASTree>();
            Attributes = new BitArray((int)Attr.SIZE);
            Value = "";
        }

        public ASTree Adopt(params ASTree[] childrens)
        {
            for(int i = 0; i < childrens.Length; i++)
            {
                Children.Add(childrens[i]);
            }
            return this;
        }

        public override string ToString()
        {
            return ToStringRec(0);
        }

        private string ToStringRec(int depth)
        {
            string result = "";
            for(int i = 0; i < depth; i++)
            {
                result += "|   ";
            }
            result += LexInfo + " " + Linenr + ":" + Offset + " " + Token + " : " + Value + "\n";
            for(int i = 0; i < Children.Count; i++)
            {
                result += Children[i].ToStringRec(depth + 1);
            }
            return result;
        }
    }

    public partial class Scanner
    {
        public int LexerReturn(Tokens symbol)
        {
            yylval = new ASTree(symbol, 0, 0, yytext);
            return (int)symbol;
        }

        public int LexerReturn(int symbol)
        {
            return LexerReturn((Tokens)symbol);
        }

        public void LexerNewLine()
        {

        }

        public void LexerBadToken(string lexeme)
        {
            System.Diagnostics.Debug.WriteLine("Error: " + lexeme);
        }
    }

    public partial class Parser
    {
        public Parser(Scanner s) : base(s) { }

        public ASTree ParserRoot;

        public ASTree ParserNewRoot(Tokens root)
        {
            return new ASTree(root, 0, 0, "");
        }
    }

    public class Symbol
    {
        public int Linenr { get; set; }
        public int Offset { get; set; }
        public string LexInfo { get; set; }
        public BitArray Attributes { get; set; }
        public string Value { get; set; }

        public Symbol(string lexinfo, int linenr, int offset, BitArray attributes, string value)
        {
            LexInfo = lexinfo;
            Linenr = linenr;
            Offset = offset;
            Attributes = attributes;
            Value = value;
        }

        public Symbol(ASTree node) : this(node.LexInfo, node.Linenr, node.Offset, node.Attributes, node.Value) { }
    }

    public class SymbolTable
    {
        Dictionary<string, Symbol> SymTable;
        public SymbolTable()
        {
            SymTable = new Dictionary<string, Symbol>();
        }

        public Symbol InsertSymbol(ASTree node)
        {
            if(SymTable.ContainsKey(node.LexInfo))
            {
                return null;
            }
            node.Symbol = new Symbol(node);
            SymTable.Add(node.LexInfo, node.Symbol);
            return node.Symbol;
        }

        public Symbol InsertSymbol(string lexinfo, Attr attr, string value)
        {
            BitArray attribute = new BitArray((int)Attr.SIZE);
            attribute.Set((int)attr, true);
            if (SymTable.ContainsKey(lexinfo))
            {
                return null;
            }
            Symbol Sym = new Symbol(lexinfo, 0, 0, attribute, value);
            SymTable.Add(lexinfo, Sym);
            return Sym;
        }

        public Symbol Get(ASTree node)
        {
            if (SymTable.ContainsKey(node.LexInfo))
            {
                return SymTable[node.LexInfo];
            }
            else
                return null;
        }
    }

    static class Interpreter
    {
        private static RobotBrain robot;
        private static SymbolTable symtable;
        private static SymbolTable robottable;
        private static bool error;
        public static void RunInterpreter(RobotBrain rb, string input)
        {
            error = false;
            Scanner scanner = new Scanner();
            scanner.SetSource(input, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
			robot = rb;
            symtable = new SymbolTable();
            robottable = new SymbolTable();
            robottable.InsertSymbol("walkSpeed", Attr.INT, "1");
            robottable.InsertSymbol("willJump", Attr.BOOL, "false");
            SemanticAnalysis(parser.ParserRoot);
            Console.print(parser.ParserRoot);
            if (!error)
            {
                Interpret(parser.ParserRoot);
            }
        }

        private static void SetRobot()
        {
            robot = (RobotBrain)GameObject.Find("Robot").GetComponent("RobotBrain");
        }

        private static void SemanticAnalysis(ASTree node)
        {
            switch (node.Token)
            {
                case Tokens.VARDECL:
                    ASTree id = node.Children[0].Children[0];
                    switch (node.Children[0].Token)
                    {
                        case Tokens.BOOL:
                            node.Children[0].Attributes.Set((int)Attr.BOOL, true);
                            id.Attributes.Set((int)Attr.BOOL, true);
                            break;
                        case Tokens.CHAR:
                            node.Children[0].Attributes.Set((int)Attr.CHAR, true);
                            id.Attributes.Set((int)Attr.CHAR, true);
                            break;
                        case Tokens.FLOAT:
                            node.Children[0].Attributes.Set((int)Attr.FLOAT, true);
                            id.Attributes.Set((int)Attr.FLOAT, true);
                            break;
                        case Tokens.INT:
                            node.Children[0].Attributes.Set((int)Attr.INT, true);
                            id.Attributes.Set((int)Attr.INT, true);
                            break;
                        case Tokens.STRING:
                            node.Children[0].Attributes.Set((int)Attr.STRING, true);
                            id.Attributes.Set((int)Attr.STRING, true);
                            break;
                    }
                    Symbol symbol = symtable.InsertSymbol(id);
                    break;
            }
            foreach (ASTree child in node.Children)
            {
                SemanticAnalysis(child);
            }
            switch(node.Token)
            {
                case Tokens.VARDECL:
                    node.Children[0].Children[0].Value = node.Children[1].Value;
                    break;
                case Tokens.IDENT:
                    TCIdent(node);
                    break;
                case Tokens.FIELD:
                    TCField(node);
                    break;
                case (Tokens)'=':
                    TCAssignment(node);
                    break;
                case (Tokens)'+':
                case (Tokens)'-':
                case (Tokens)'*':
                case (Tokens)'/':
                    TCBinop(node);
                    break;
                case Tokens.POS:
                case Tokens.NEG:
                    TCUnop(node);
                    break;
                case Tokens.TRUE:
                case Tokens.FALSE:
                    node.Attributes.Set((int)Attr.CONST, true);
                    node.Value = node.LexInfo;
                    goto case Tokens.BOOL;
                case Tokens.BOOL:
                    node.Attributes.Set((int)Attr.BOOL, true);
                    break;
                case Tokens.CHARCON:
                    node.Attributes.Set((int)Attr.CONST, true);
                    node.Value = node.LexInfo;
                    goto case Tokens.CHAR;
                case Tokens.CHAR:
                    node.Attributes.Set((int)Attr.CHAR, true);
                    break;
                case Tokens.FLOATCON:
                    node.Attributes.Set((int)Attr.CONST, true);
                    node.Value = node.LexInfo;
                    goto case Tokens.FLOAT;
                case Tokens.FLOAT:
                    node.Attributes.Set((int)Attr.FLOAT, true);
                    break;
                case Tokens.INTCON:
                    node.Attributes.Set((int)Attr.CONST, true);
                    node.Value = node.LexInfo;
                    goto case Tokens.INT;
                case Tokens.INT:
                    node.Attributes.Set((int)Attr.INT, true);
                    break;
                case Tokens.STRINGCON:
                    node.Attributes.Set((int)Attr.CONST, true);
                    node.Value = node.LexInfo;
                    goto case Tokens.STRING;
                case Tokens.STRING:
                    node.Attributes.Set((int)Attr.STRING, true);
                    break;
            }
        }
       
        #region typechecks
        private static bool Compatible(ASTree node1, ASTree node2)
        {
            BitArray attr1 = node1.Attributes;
            BitArray attr2 = node2.Attributes;
            if (attr1.Get((int)Attr.BOOL) && !attr2.Get((int)Attr.BOOL))
            {
                return false;
            }
            if (attr1.Get((int)Attr.CHAR) && !attr2.Get((int)Attr.CHAR))
            {
                return false;
            }
            if (attr1.Get((int)Attr.FLOAT) && !attr2.Get((int)Attr.FLOAT))
            {
                return false;
            }
            if (attr1.Get((int)Attr.INT) && !attr2.Get((int)Attr.INT))
            {
                return false;
            }
            if (attr1.Get((int)Attr.STRING) && !attr2.Get((int)Attr.STRING))
            {
                return false;
            }
            return true;
        }

        private static void TCIdent(ASTree node)
        {
            Symbol sym = symtable.Get(node);
            if (sym == null)
            {
                error = true;
                return;
            }
            node.Attributes = sym.Attributes;
            node.Value = sym.Value;
            node.Symbol = sym;
        }

        private static void TCField(ASTree node)
        {
            Symbol sym = robottable.Get(node);
            if (sym == null)
            {
                error = true;
                Console.print("Not valid robot variable");
                return;
            }
            node.Attributes = sym.Attributes;
            node.Value = sym.Value;
            node.Symbol = sym;
        }

        private static void TCAssignment(ASTree node)
        {
            ASTree left = node.Children[0];
            ASTree right = node.Children[1];
            /*if(!left.Attributes.Get((int)Attr.LVAL))
            {
                error = true;
                return;
            }*/
            if(!Compatible(left, right))
            {
                error = true;
                Console.print("Cannot assign variable");
                return;
            }
            left.Value = right.Value;
        }

        private static void TCBinop(ASTree node)
        {
            ASTree left = node.Children[0];
            ASTree right = node.Children[1];
            if(!Compatible(left, right))
            {
                error = true;
                Console.print("Not compatible");
                return;
            }
            if(!left.Attributes.Get((int)Attr.INT) && !left.Attributes.Get((int)Attr.FLOAT))
            {
                error = true;
                Console.print("Cannot do operator");
                return;
            }
            if(left.Attributes.Get((int)Attr.INT))
            {
                int temp1 = int.Parse(left.Value);
                int temp2 = int.Parse(right.Value);
                int temp = 0;
                switch(node.Token)
                {
                    case (Tokens)'+':
                        temp = temp1 + temp2;
                        break;
                    case (Tokens)'-':
                        temp = temp1 - temp2;
                        break;
                    case (Tokens)'*':
                        temp = temp1 * temp2;
                        break;
                    case (Tokens)'/':
                        temp = temp1 / temp2;
                        break;
                }
                node.Value = temp.ToString();
                node.Attributes.Set((int)Attr.INT, true);
            } else if(left.Attributes.Get((int)Attr.FLOAT))
            {
                float temp1 = float.Parse(left.Value);
                float temp2 = float.Parse(right.Value);
                float temp = 0;
                switch (node.Token)
                {
                    case (Tokens)'+':
                        temp = temp1 + temp2;
                        break;
                    case (Tokens)'-':
                        temp = temp1 - temp2;
                        break;
                    case (Tokens)'*':
                        temp = temp1 * temp2;
                        break;
                    case (Tokens)'/':
                        temp = temp1 / temp2;
                        break;
                }
                node.Value = temp.ToString();
                node.Attributes.Set((int)Attr.FLOAT, true);
            }
        }

        private static void TCUnop(ASTree node)
        {
            if (!node.Attributes.Get((int)Attr.INT) && !node.Attributes.Get((int)Attr.FLOAT))
            {
                error = true;
                return;
            }
            if(node.Token == Tokens.NEG)
            {
                node.Value = (float.Parse(node.Children[0].Value) * -1).ToString();
            } else
            {
                node.Value = node.Children[0].Value;
            }
        }

        private static void TCCall(ASTree node)
        {
            ASTree func = node.Children[0];
            Symbol sym = robottable.Get(func);
            if(sym == null)
            {
                error = true;
                return;
            }
            node.Attributes = sym.Attributes;
            node.Value = sym.Value;
            node.Symbol = sym;
        }
        #endregion

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        private static void Interpret(ASTree root)
        {
            foreach(ASTree child in root.Children)
            {
                switch(child.Token)
                {
                    case (Tokens)'=':
                        switch (child.Children[0].LexInfo)
                        {
                            case "walkSpeed":
							    robot.setSpeed("Normal",int.Parse(child.Children[1].Value));
                                break;
                            case "willJump":
                                robot.canJump("Normal", bool.Parse(FirstLetterToUpper(child.Children[1].Value)), 10);
                                break;
                        }
                        break;
                    default:
                        Interpret(child);
                        break;
                }
            }
        }
    }
}
