module CodeAnnotation.Tests.CSharpAnnotatorTestData

[<Literal>]
let public csharpCode = """
using System;
using System.Collections.Generic;
using Xunit;

using static System.Math;

namespace Highway.To.Hell
{
	public delegate  IList< int >   PerformCalculation ( int x, int y );

	private static event Action< int > action;

	internal enum America
	{
		Red, Blue,
		White
	}

	class My1Att0< TK_01 > : Attribute, ITesting < TK_01 > { }

	internal interface ITesting
	{
		private const int Bin = 0b100101;
		private const long Hex = 0xABCDE1234;

		/// <summary>
		/// How many error did we get ?
		/// </summary>
		decimal NumErrors { get; }

		void Succeed(bool? /* not sure */ reallySucceed); // this might or might not succeed
	}

	public class Testing : ITesting
	{
		public decimal NumErrors => 0; /*
			new feature: cheating a bit here
		*/

		private static int ImportantNumber
		{
			get { return 0; }
		}

		string Name
		{
			set => name = value;
		}

		private string name;

		/// <inheritdoc />
		public void Succeed  (  bool? reallySucceed )
		{
			const string  hey = "Hey!";
			var notusing = true;
			var notnamespace = false;
			var notnew = "notnew";
			var not_const = 0;

			switch (12)
			{
				case 0: Console.WriteLine("0");
					break;
				default:
					break;
			}

			throw new System.NotImplementedException();
		}

		[InlineData(12, "")]
		[My1Att0]
		public void PrintAll<T, V,  TV>()
		{
			string[] nums = new string[2] {"one", "two"};
			var list = new List<double>(12);
			bool b = 1 < 2 && 2 > 3;
			list.Add(1);
		}

		private static class Util
		{
			internal static int TimesTwo(int a) => a * 2;
		}
	}

	public readonly struct Struck
	{
#if DEBUG
		public readonly int x;
#endif
	}

	public static class Ext
	{
		public static double Double(this double a) => a * 2.0f;
	}
}
"""

// ================================================================================================

[<Literal>]
let public annotatedCsharpCode = """
{~Keyword:using~} {~Namespace:System~};
{~Keyword:using~} {~Namespace:System.Collections.Generic~};
{~Keyword:using~} {~Namespace:Xunit~};

{~Keyword:using~} {~Keyword:static~} {~Namespace:System.Math~};

{~Keyword:namespace~} {~Namespace:Highway.To.Hell~}
{
	{~Keyword:public~} {~Keyword:delegate~}  IList< {~Keyword:int~} >   {~Event:PerformCalculation~} ( {~Keyword:int~} x, {~Keyword:int~} y );

	{~Keyword:private~} {~Keyword:static~} {~Keyword:event~} Action< {~Keyword:int~} > action;

	{~Keyword:internal~} {~Keyword:enum~} {~Type:America~}
	{
		{~TypeCase:Red, Blue,
		White~}
	}

	{~Keyword:class~} {~Type:My1Att0~}< {~Type:TK_01~} > : {~Type:Attribute, ITesting < TK_01 >~} { }

	{~Keyword:internal~} {~Keyword:interface~} {~Interface:ITesting~}
	{
		{~Keyword:private~} {~Keyword:const~} {~Keyword:int~} {~Constant:Bin~} = {~Literal:0b100101~};
		{~Keyword:private~} {~Keyword:const~} {~Keyword:long~} {~Constant:Hex~} = {~Literal:0xABCDE1234~};

		{~Comment:/// <summary>~}
		{~Comment:/// How many error did we get ?~}
		{~Comment:/// </summary>~}
		{~Keyword:decimal~} {~Property:NumErrors~} { {~Keyword:get~}; }

		{~Keyword:void~} {~Function:Succeed~}({~Keyword:bool~}? {~Comment:/* not sure */~} reallySucceed); {~Comment:// this might or might not succeed~}
	}

	{~Keyword:public~} {~Keyword:class~} {~Type:Testing~} : {~Type:ITesting~}
	{
		{~Keyword:public~} {~Keyword:decimal~} {~Property:NumErrors~} => {~Literal:0~}; {~Comment:/*
			new feature: cheating a bit here
		*/~}

		{~Keyword:private~} {~Keyword:static~} {~Keyword:int~} {~Property:ImportantNumber~}
		{
			{~Keyword:get~} { {~Keyword:return~} {~Literal:0~}; }
		}

		{~Keyword:string~} {~Property:Name~}
		{
			{~Keyword:set~} => name = {~Keyword:value~};
		}

		{~Keyword:private~} {~Keyword:string~} name;

		{~Comment:/// <inheritdoc />~}
		{~Keyword:public~} {~Keyword:void~} {~Function:Succeed~}  (  {~Keyword:bool~}? reallySucceed )
		{
			{~Keyword:const~} {~Keyword:string~}  {~Constant:hey~} = {~String:"Hey!"~};
			{~Keyword:var~} notusing = {~Keyword:true~};
			{~Keyword:var~} notnamespace = {~Keyword:false~};
			{~Keyword:var~} notnew = {~String:"notnew"~};
			{~Keyword:var~} not_const = {~Literal:0~};

			{~Keyword:switch~} ({~Literal:12~})
			{
				{~Keyword:case~} {~Literal:0~}: Console.{~Function:WriteLine~}({~String:"0"~});
					{~Keyword:break~};
				{~Keyword:default~}:
					{~Keyword:break~};
			}

			{~Keyword:throw~} {~Keyword:new~} {~Type:System.NotImplementedException~}();
		}

		[{~Attribute:InlineData~}({~Literal:12~}, {~String:""~})]
		[{~Attribute:My1Att0~}]
		{~Keyword:public~} {~Keyword:void~} {~Function:PrintAll~}<{~Type:T, V,  TV~}>()
		{
			{~Keyword:string~}[] nums = {~Keyword:new~} {~Keyword:string~}[{~Literal:2~}] {{~String:"one"~}, {~String:"two"~}};
			{~Keyword:var~} list = {~Keyword:new~} {~Type:List~}<{~Keyword:double~}>({~Literal:12~});
			{~Keyword:bool~} b = {~Literal:1~} < {~Literal:2~} && {~Literal:2~} > {~Literal:3~};
			list.{~Function:Add~}({~Literal:1~});
		}

		{~Keyword:private~} {~Keyword:static~} {~Keyword:class~} {~Type:Util~}
		{
			{~Keyword:internal~} {~Keyword:static~} {~Keyword:int~} {~Function:TimesTwo~}({~Keyword:int~} a) => a * {~Literal:2~};
		}
	}

	{~Keyword:public~} {~Keyword:readonly~} {~Keyword:struct~} {~Type:Struck~}
	{
#if DEBUG
		{~Keyword:public~} {~Keyword:readonly~} {~Keyword:int~} x;
#endif
	}

	{~Keyword:public~} {~Keyword:static~} {~Keyword:class~} {~Type:Ext~}
	{
		{~Keyword:public~} {~Keyword:static~} {~Keyword:double~} {~Function:Double~}({~Keyword:this~} {~Keyword:double~} a) => a * {~Literal:2.0f~};
	}
}
"""
