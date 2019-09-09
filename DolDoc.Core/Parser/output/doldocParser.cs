//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 3.5.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// $ANTLR 3.5.1 C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g 2019-09-09 15:13:16

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 219
// Unreachable code detected.
#pragma warning disable 162
// Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable 1591
// CLS compliance checking will not be performed on 'type' because it is not visible from outside this assembly.
#pragma warning disable 3019


using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Misc;

namespace  DolDoc.Core.Parser 
{
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "3.5.1")]
[System.CLSCompliant(false)]
public partial class doldocParser : Antlr.Runtime.Parser
{
	internal static readonly string[] tokenNames = new string[] {
		"<invalid>", "<EOR>", "<DOWN>", "<UP>", "Anything", "COMMA", "DOLLAR", "EQ", "Identifier", "MINUS", "Number", "PLUS", "QUOTE", "String"
	};
	public const int EOF=-1;
	public const int Anything=4;
	public const int COMMA=5;
	public const int DOLLAR=6;
	public const int EQ=7;
	public const int Identifier=8;
	public const int MINUS=9;
	public const int Number=10;
	public const int PLUS=11;
	public const int QUOTE=12;
	public const int String=13;

	public doldocParser(ITokenStream input)
		: this(input, new RecognizerSharedState())
	{
	}
	public doldocParser(ITokenStream input, RecognizerSharedState state)
		: base(input, state)
	{
		OnCreated();
	}

	public override string[] TokenNames { get { return doldocParser.tokenNames; } }
	public override string GrammarFileName { get { return "C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g"; } }


	partial void OnCreated();
	partial void EnterRule(string ruleName, int ruleIndex);
	partial void LeaveRule(string ruleName, int ruleIndex);

	#region Rules
	partial void EnterRule_document();
	partial void LeaveRule_document();
	// $ANTLR start "document"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:24:8: public document returns [IList<DocumentNode> nodes = new List<DocumentNode>(); ] : (n= document_node )+ ;
	[GrammarRule("document")]
	public IList<DocumentNode> document()
	{
		EnterRule_document();
		EnterRule("document", 1);
		TraceIn("document", 1);
		IList<DocumentNode> nodes =  new List<DocumentNode>();;


		DocumentNode n = default(DocumentNode);

		try { DebugEnterRule(GrammarFileName, "document");
		DebugLocation(24, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:25:2: ( (n= document_node )+ )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:25:4: (n= document_node )+
			{
			DebugLocation(25, 4);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:25:4: (n= document_node )+
			int cnt1=0;
			try { DebugEnterSubRule(1);
			while (true)
			{
				int alt1=2;
				try { DebugEnterDecision(1, false);
				int LA1_1 = input.LA(1);

				if (((LA1_1>=Anything && LA1_1<=String)))
				{
					alt1 = 1;
				}


				} finally { DebugExitDecision(1); }
				switch (alt1)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:25:5: n= document_node
					{
					DebugLocation(25, 6);
					PushFollow(Follow._document_node_in_document157);
					n=document_node();
					PopFollow();

					DebugLocation(25, 21);
					 nodes.Add(n); 

					}
					break;

				default:
					if (cnt1 >= 1)
						goto loop1;

					EarlyExitException eee1 = new EarlyExitException( 1, input );
					DebugRecognitionException(eee1);
					throw eee1;
				}
				cnt1++;
			}
			loop1:
				;

			} finally { DebugExitSubRule(1); }


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("document", 1);
			LeaveRule("document", 1);
			LeaveRule_document();
		}
		DebugLocation(26, 1);
		} finally { DebugExitRule(GrammarFileName, "document"); }
		return nodes;

	}
	// $ANTLR end "document"

	partial void EnterRule_document_node();
	partial void LeaveRule_document_node();
	// $ANTLR start "document_node"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:28:1: document_node returns [DocumentNode node] : ( DOLLAR DOLLAR | command | . );
	[GrammarRule("document_node")]
	private DocumentNode document_node()
	{
		EnterRule_document_node();
		EnterRule("document_node", 2);
		TraceIn("document_node", 2);
		DocumentNode node = default(DocumentNode);


		Command command1 = default(Command);

		try { DebugEnterRule(GrammarFileName, "document_node");
		DebugLocation(28, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:29:2: ( DOLLAR DOLLAR | command | . )
			int alt2=3;
			try { DebugEnterDecision(2, false);
			try
			{
				alt2 = dfa2.Predict(input);
			}
			catch (NoViableAltException nvae)
			{
				DebugRecognitionException(nvae);
				throw;
			}
			} finally { DebugExitDecision(2); }
			switch (alt2)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:29:4: DOLLAR DOLLAR
				{
				DebugLocation(29, 4);
				Match(input,DOLLAR,Follow._DOLLAR_in_document_node177); 
				DebugLocation(29, 11);
				Match(input,DOLLAR,Follow._DOLLAR_in_document_node179); 
				DebugLocation(29, 23);
				 node = new StringNode("$"); 

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:30:4: command
				{
				DebugLocation(30, 4);
				PushFollow(Follow._command_in_document_node191);
				command1=command();
				PopFollow();

				DebugLocation(30, 18);
				 node = command1; 

				}
				break;
			case 3:
				DebugEnterAlt(3);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:31:4: .
				{
				DebugLocation(31, 4);
				MatchAny(input); 
				DebugLocation(31, 12);
				 node = new StringNode(input.LT(1).Text); 

				}
				break;

			}
		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("document_node", 2);
			LeaveRule("document_node", 2);
			LeaveRule_document_node();
		}
		DebugLocation(32, 1);
		} finally { DebugExitRule(GrammarFileName, "document_node"); }
		return node;

	}
	// $ANTLR end "document_node"

	partial void EnterRule_command();
	partial void LeaveRule_command();
	// $ANTLR start "command"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:34:1: command returns [Command cmd] : DOLLAR Identifier ( flag_list )? ( argument_list )? DOLLAR ;
	[GrammarRule("command")]
	private Command command()
	{
		EnterRule_command();
		EnterRule("command", 3);
		TraceIn("command", 3);
		Command cmd = default(Command);


		IToken Identifier2 = default(IToken);
		IList<Flag> flag_list3 = default(IList<Flag>);
		IList<Argument> argument_list4 = default(IList<Argument>);

		try { DebugEnterRule(GrammarFileName, "command");
		DebugLocation(34, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:2: ( DOLLAR Identifier ( flag_list )? ( argument_list )? DOLLAR )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:4: DOLLAR Identifier ( flag_list )? ( argument_list )? DOLLAR
			{
			DebugLocation(35, 4);
			Match(input,DOLLAR,Follow._DOLLAR_in_command228); 
			DebugLocation(35, 11);
			Identifier2=(IToken)Match(input,Identifier,Follow._Identifier_in_command230); 
			DebugLocation(35, 22);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:22: ( flag_list )?
			int alt3=2;
			try { DebugEnterSubRule(3);
			try { DebugEnterDecision(3, false);
			int LA3_1 = input.LA(1);

			if ((LA3_1==MINUS||LA3_1==PLUS))
			{
				alt3 = 1;
			}
			} finally { DebugExitDecision(3); }
			switch (alt3)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:22: flag_list
				{
				DebugLocation(35, 22);
				PushFollow(Follow._flag_list_in_command232);
				flag_list3=flag_list();
				PopFollow();


				}
				break;

			}
			} finally { DebugExitSubRule(3); }

			DebugLocation(35, 33);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:33: ( argument_list )?
			int alt4=2;
			try { DebugEnterSubRule(4);
			try { DebugEnterDecision(4, false);
			int LA4_1 = input.LA(1);

			if ((LA4_1==COMMA))
			{
				alt4 = 1;
			}
			} finally { DebugExitDecision(4); }
			switch (alt4)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:35:33: argument_list
				{
				DebugLocation(35, 33);
				PushFollow(Follow._argument_list_in_command235);
				argument_list4=argument_list();
				PopFollow();


				}
				break;

			}
			} finally { DebugExitSubRule(4); }

			DebugLocation(35, 48);
			Match(input,DOLLAR,Follow._DOLLAR_in_command238); 
			DebugLocation(35, 55);
			 cmd = new Command((Identifier2!=null?Identifier2.Text:default(string)), flag_list3, argument_list4); 

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("command", 3);
			LeaveRule("command", 3);
			LeaveRule_command();
		}
		DebugLocation(36, 1);
		} finally { DebugExitRule(GrammarFileName, "command"); }
		return cmd;

	}
	// $ANTLR end "command"

	partial void EnterRule_argument_expr();
	partial void LeaveRule_argument_expr();
	// $ANTLR start "argument_expr"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:38:1: argument_expr : ( String | Number | Identifier );
	[GrammarRule("argument_expr")]
	private ParserRuleReturnScope<IToken> argument_expr()
	{
		EnterRule_argument_expr();
		EnterRule("argument_expr", 4);
		TraceIn("argument_expr", 4);
		ParserRuleReturnScope<IToken> retval = new ParserRuleReturnScope<IToken>();
		retval.Start = (IToken)input.LT(1);

		try { DebugEnterRule(GrammarFileName, "argument_expr");
		DebugLocation(38, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:39:2: ( String | Number | Identifier )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:
			{
			DebugLocation(39, 2);
			if (input.LA(1)==Identifier||input.LA(1)==Number||input.LA(1)==String)
			{
				input.Consume();
				state.errorRecovery=false;
			}
			else
			{
				MismatchedSetException mse = new MismatchedSetException(null,input);
				DebugRecognitionException(mse);
				throw mse;
			}


			}

			retval.Stop = (IToken)input.LT(-1);

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("argument_expr", 4);
			LeaveRule("argument_expr", 4);
			LeaveRule_argument_expr();
		}
		DebugLocation(42, 1);
		} finally { DebugExitRule(GrammarFileName, "argument_expr"); }
		return retval;

	}
	// $ANTLR end "argument_expr"

	partial void EnterRule_argument();
	partial void LeaveRule_argument();
	// $ANTLR start "argument"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:45:1: argument returns [Argument arg] : COMMA ( Identifier EQ )? argument_expr ;
	[GrammarRule("argument")]
	private Argument argument()
	{
		EnterRule_argument();
		EnterRule("argument", 5);
		TraceIn("argument", 5);
		Argument arg = default(Argument);


		IToken Identifier5 = default(IToken);
		ParserRuleReturnScope<IToken> argument_expr6 = default(ParserRuleReturnScope<IToken>);

		try { DebugEnterRule(GrammarFileName, "argument");
		DebugLocation(45, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:46:2: ( COMMA ( Identifier EQ )? argument_expr )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:46:4: COMMA ( Identifier EQ )? argument_expr
			{
			DebugLocation(46, 4);
			Match(input,COMMA,Follow._COMMA_in_argument281); 
			DebugLocation(46, 10);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:46:10: ( Identifier EQ )?
			int alt5=2;
			try { DebugEnterSubRule(5);
			try { DebugEnterDecision(5, false);
			int LA5_1 = input.LA(1);

			if ((LA5_1==Identifier))
			{
				int LA5_2 = input.LA(2);

				if ((LA5_2==EQ))
				{
					alt5 = 1;
				}
			}
			} finally { DebugExitDecision(5); }
			switch (alt5)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:46:11: Identifier EQ
				{
				DebugLocation(46, 11);
				Identifier5=(IToken)Match(input,Identifier,Follow._Identifier_in_argument284); 
				DebugLocation(46, 22);
				Match(input,EQ,Follow._EQ_in_argument286); 

				}
				break;

			}
			} finally { DebugExitSubRule(5); }

			DebugLocation(46, 27);
			PushFollow(Follow._argument_expr_in_argument290);
			argument_expr6=argument_expr();
			PopFollow();

			DebugLocation(46, 43);
			 arg = new Argument((Identifier5!=null?Identifier5.Text:default(string)), (argument_expr6!=null?input.ToString(argument_expr6.Start,argument_expr6.Stop):default(string))); 

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("argument", 5);
			LeaveRule("argument", 5);
			LeaveRule_argument();
		}
		DebugLocation(47, 1);
		} finally { DebugExitRule(GrammarFileName, "argument"); }
		return arg;

	}
	// $ANTLR end "argument"

	partial void EnterRule_argument_list();
	partial void LeaveRule_argument_list();
	// $ANTLR start "argument_list"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:49:1: argument_list returns [IList<Argument> args = new List<Argument>(); ] : (a= argument )+ ;
	[GrammarRule("argument_list")]
	private IList<Argument> argument_list()
	{
		EnterRule_argument_list();
		EnterRule("argument_list", 6);
		TraceIn("argument_list", 6);
		IList<Argument> args =  new List<Argument>();;


		Argument a = default(Argument);

		try { DebugEnterRule(GrammarFileName, "argument_list");
		DebugLocation(49, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:50:2: ( (a= argument )+ )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:50:4: (a= argument )+
			{
			DebugLocation(50, 4);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:50:4: (a= argument )+
			int cnt6=0;
			try { DebugEnterSubRule(6);
			while (true)
			{
				int alt6=2;
				try { DebugEnterDecision(6, false);
				int LA6_1 = input.LA(1);

				if ((LA6_1==COMMA))
				{
					alt6 = 1;
				}


				} finally { DebugExitDecision(6); }
				switch (alt6)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:50:5: a= argument
					{
					DebugLocation(50, 6);
					PushFollow(Follow._argument_in_argument_list313);
					a=argument();
					PopFollow();

					DebugLocation(50, 16);
					 args.Add(a); 

					}
					break;

				default:
					if (cnt6 >= 1)
						goto loop6;

					EarlyExitException eee6 = new EarlyExitException( 6, input );
					DebugRecognitionException(eee6);
					throw eee6;
				}
				cnt6++;
			}
			loop6:
				;

			} finally { DebugExitSubRule(6); }


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("argument_list", 6);
			LeaveRule("argument_list", 6);
			LeaveRule_argument_list();
		}
		DebugLocation(51, 1);
		} finally { DebugExitRule(GrammarFileName, "argument_list"); }
		return args;

	}
	// $ANTLR end "argument_list"

	partial void EnterRule_flag_status();
	partial void LeaveRule_flag_status();
	// $ANTLR start "flag_status"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:53:1: flag_status returns [bool status] : ( PLUS | MINUS );
	[GrammarRule("flag_status")]
	private bool flag_status()
	{
		EnterRule_flag_status();
		EnterRule("flag_status", 7);
		TraceIn("flag_status", 7);
		bool status = default(bool);


		try { DebugEnterRule(GrammarFileName, "flag_status");
		DebugLocation(53, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:54:2: ( PLUS | MINUS )
			int alt7=2;
			try { DebugEnterDecision(7, false);
			int LA7_1 = input.LA(1);

			if ((LA7_1==PLUS))
			{
				alt7 = 1;
			}
			else if ((LA7_1==MINUS))
			{
				alt7 = 2;
			}
			else
			{
				NoViableAltException nvae = new NoViableAltException("", 7, 0, input, 1);
				DebugRecognitionException(nvae);
				throw nvae;
			}
			} finally { DebugExitDecision(7); }
			switch (alt7)
			{
			case 1:
				DebugEnterAlt(1);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:54:4: PLUS
				{
				DebugLocation(54, 4);
				Match(input,PLUS,Follow._PLUS_in_flag_status333); 
				DebugLocation(54, 15);
				 status = true; 

				}
				break;
			case 2:
				DebugEnterAlt(2);
				// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:55:4: MINUS
				{
				DebugLocation(55, 4);
				Match(input,MINUS,Follow._MINUS_in_flag_status346); 
				DebugLocation(55, 16);
				 status = false; 

				}
				break;

			}
		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("flag_status", 7);
			LeaveRule("flag_status", 7);
			LeaveRule_flag_status();
		}
		DebugLocation(56, 1);
		} finally { DebugExitRule(GrammarFileName, "flag_status"); }
		return status;

	}
	// $ANTLR end "flag_status"

	partial void EnterRule_flag();
	partial void LeaveRule_flag();
	// $ANTLR start "flag"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:58:1: flag returns [Flag flag] : flag_status Identifier ;
	[GrammarRule("flag")]
	private Flag flag()
	{
		EnterRule_flag();
		EnterRule("flag", 8);
		TraceIn("flag", 8);
		Flag flag = default(Flag);


		IToken Identifier7 = default(IToken);
		bool flag_status8 = default(bool);

		try { DebugEnterRule(GrammarFileName, "flag");
		DebugLocation(58, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:59:2: ( flag_status Identifier )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:59:4: flag_status Identifier
			{
			DebugLocation(59, 4);
			PushFollow(Follow._flag_status_in_flag369);
			flag_status8=flag_status();
			PopFollow();

			DebugLocation(59, 16);
			Identifier7=(IToken)Match(input,Identifier,Follow._Identifier_in_flag371); 
			DebugLocation(59, 31);
			 flag = new Flag((Identifier7!=null?Identifier7.Text:default(string)), flag_status8); 

			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("flag", 8);
			LeaveRule("flag", 8);
			LeaveRule_flag();
		}
		DebugLocation(60, 1);
		} finally { DebugExitRule(GrammarFileName, "flag"); }
		return flag;

	}
	// $ANTLR end "flag"

	partial void EnterRule_flag_list();
	partial void LeaveRule_flag_list();
	// $ANTLR start "flag_list"
	// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:62:1: flag_list returns [IList<Flag> flags = new List<Flag>(); ] : (f= flag )+ ;
	[GrammarRule("flag_list")]
	private IList<Flag> flag_list()
	{
		EnterRule_flag_list();
		EnterRule("flag_list", 9);
		TraceIn("flag_list", 9);
		IList<Flag> flags =  new List<Flag>();;


		Flag f = default(Flag);

		try { DebugEnterRule(GrammarFileName, "flag_list");
		DebugLocation(62, 1);
		try
		{
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:63:2: ( (f= flag )+ )
			DebugEnterAlt(1);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:63:4: (f= flag )+
			{
			DebugLocation(63, 4);
			// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:63:4: (f= flag )+
			int cnt8=0;
			try { DebugEnterSubRule(8);
			while (true)
			{
				int alt8=2;
				try { DebugEnterDecision(8, false);
				int LA8_1 = input.LA(1);

				if ((LA8_1==MINUS||LA8_1==PLUS))
				{
					alt8 = 1;
				}


				} finally { DebugExitDecision(8); }
				switch (alt8)
				{
				case 1:
					DebugEnterAlt(1);
					// C:\\Projects\\_pr\\DolDoc.NET\\DolDoc.Core\\Parser\\doldoc.g:63:5: f= flag
					{
					DebugLocation(63, 6);
					PushFollow(Follow._flag_in_flag_list396);
					f=flag();
					PopFollow();

					DebugLocation(63, 12);
					 flags.Add(f); 

					}
					break;

				default:
					if (cnt8 >= 1)
						goto loop8;

					EarlyExitException eee8 = new EarlyExitException( 8, input );
					DebugRecognitionException(eee8);
					throw eee8;
				}
				cnt8++;
			}
			loop8:
				;

			} finally { DebugExitSubRule(8); }


			}

		}
		catch (RecognitionException re)
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
			TraceOut("flag_list", 9);
			LeaveRule("flag_list", 9);
			LeaveRule_flag_list();
		}
		DebugLocation(64, 1);
		} finally { DebugExitRule(GrammarFileName, "flag_list"); }
		return flags;

	}
	// $ANTLR end "flag_list"
	#endregion Rules


	#region DFA
	private DFA2 dfa2;

	protected override void InitDFAs()
	{
		base.InitDFAs();
		dfa2 = new DFA2( this );
	}

	private class DFA2 : DFA
	{
		private const string DFA2_eotS =
			"\xD\xFFFF";
		private const string DFA2_eofS =
			"\x1\xFFFF\x1\x2\x2\xFFFF\x4\x2\x1\xFFFF\x4\x2";
		private const string DFA2_minS =
			"\x2\x4\x2\xFFFF\x4\x4\x1\xFFFF\x4\x4";
		private const string DFA2_maxS =
			"\x2\xD\x2\xFFFF\x4\xD\x1\xFFFF\x4\xD";
		private const string DFA2_acceptS =
			"\x2\xFFFF\x1\x3\x1\x1\x4\xFFFF\x1\x2\x4\xFFFF";
		private const string DFA2_specialS =
			"\xD\xFFFF}>";
		private static readonly string[] DFA2_transitionS =
			{
				"\x2\x2\x1\x1\x7\x2",
				"\x2\x2\x1\x3\x1\x2\x1\x4\x5\x2",
				"",
				"",
				"\x1\x2\x1\x7\x1\x8\x2\x2\x1\x6\x1\x2\x1\x5\x2\x2",
				"\x4\x2\x1\x9\x5\x2",
				"\x4\x2\x1\x9\x5\x2",
				"\x4\x2\x1\xA\x1\x2\x1\xB\x2\x2\x1\xB",
				"",
				"\x1\x2\x1\x7\x1\x8\x2\x2\x1\x6\x1\x2\x1\x5\x2\x2",
				"\x1\x2\x1\x7\x1\x8\x1\xC\x6\x2",
				"\x1\x2\x1\x7\x1\x8\x7\x2",
				"\x4\x2\x1\xB\x1\x2\x1\xB\x2\x2\x1\xB"
			};

		private static readonly short[] DFA2_eot = DFA.UnpackEncodedString(DFA2_eotS);
		private static readonly short[] DFA2_eof = DFA.UnpackEncodedString(DFA2_eofS);
		private static readonly char[] DFA2_min = DFA.UnpackEncodedStringToUnsignedChars(DFA2_minS);
		private static readonly char[] DFA2_max = DFA.UnpackEncodedStringToUnsignedChars(DFA2_maxS);
		private static readonly short[] DFA2_accept = DFA.UnpackEncodedString(DFA2_acceptS);
		private static readonly short[] DFA2_special = DFA.UnpackEncodedString(DFA2_specialS);
		private static readonly short[][] DFA2_transition;

		static DFA2()
		{
			int numStates = DFA2_transitionS.Length;
			DFA2_transition = new short[numStates][];
			for ( int i=0; i < numStates; i++ )
			{
				DFA2_transition[i] = DFA.UnpackEncodedString(DFA2_transitionS[i]);
			}
		}

		public DFA2( BaseRecognizer recognizer )
		{
			this.recognizer = recognizer;
			this.decisionNumber = 2;
			this.eot = DFA2_eot;
			this.eof = DFA2_eof;
			this.min = DFA2_min;
			this.max = DFA2_max;
			this.accept = DFA2_accept;
			this.special = DFA2_special;
			this.transition = DFA2_transition;
		}

		public override string Description { get { return "28:1: document_node returns [DocumentNode node] : ( DOLLAR DOLLAR | command | . );"; } }

		public override void Error(NoViableAltException nvae)
		{
			DebugRecognitionException(nvae);
		}
	}


	#endregion DFA

	#region Follow sets
	private static class Follow
	{
		public static readonly BitSet _document_node_in_document157 = new BitSet(new ulong[]{0x3FF2UL});
		public static readonly BitSet _DOLLAR_in_document_node177 = new BitSet(new ulong[]{0x40UL});
		public static readonly BitSet _DOLLAR_in_document_node179 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _command_in_document_node191 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _DOLLAR_in_command228 = new BitSet(new ulong[]{0x100UL});
		public static readonly BitSet _Identifier_in_command230 = new BitSet(new ulong[]{0xA60UL});
		public static readonly BitSet _flag_list_in_command232 = new BitSet(new ulong[]{0x60UL});
		public static readonly BitSet _argument_list_in_command235 = new BitSet(new ulong[]{0x40UL});
		public static readonly BitSet _DOLLAR_in_command238 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _COMMA_in_argument281 = new BitSet(new ulong[]{0x2500UL});
		public static readonly BitSet _Identifier_in_argument284 = new BitSet(new ulong[]{0x80UL});
		public static readonly BitSet _EQ_in_argument286 = new BitSet(new ulong[]{0x2500UL});
		public static readonly BitSet _argument_expr_in_argument290 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _argument_in_argument_list313 = new BitSet(new ulong[]{0x22UL});
		public static readonly BitSet _PLUS_in_flag_status333 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _MINUS_in_flag_status346 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _flag_status_in_flag369 = new BitSet(new ulong[]{0x100UL});
		public static readonly BitSet _Identifier_in_flag371 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _flag_in_flag_list396 = new BitSet(new ulong[]{0xA02UL});
	}
	#endregion Follow sets
}

} // namespace  DolDoc.Core.Parser 
