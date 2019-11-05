﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.Base {

	public static class StringExtras {
		public static char[] linesSplitter = new char[1] { '\n' };
		public static char[] spaceSplitter = new char[5] { ' ', '\n', '\t', '\r', '\0' };
		//
		public static string DisplayScore(int score, int digits = 7) {
			string text = string.Empty;
			int ts = score;
			do {
				ts /= 10;
				digits--;
			} while (ts > 0);
			for (int i = digits - 1; i >= 0; --i)
				text += "0";
			return text + score;
		}
		public static string DisplayMinutes(int seconds) {
			int s = seconds % 60;
			int m = seconds / 60;
			return m.ToString() + ":" + (s < 10 ? ("0" + s) : s.ToString());
		}

		public static string CreateID(int numBlocks = 4, int lengthBlock = 4, string delimiter = "-", string possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnopqrstuvwxyzZ1234567890") {
			string code = "";
			for (int i = 0; i < numBlocks; ++i) {
				if (i != 0) code += delimiter;
				for (int j = 0; j < lengthBlock; ++j)
					code += possibleCharacters[UnityEngine.Random.Range(0, possibleCharacters.Length)].ToString();
			}
			return code;
		}

		public static string Escape(this string text) {
			if (string.IsNullOrEmpty(text)) { return text; }
			return text.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r\n", "\\n").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
		}

		// from https://stackoverflow.com/questions/2661169/how-can-i-unescape-and-reescape-strings-in-net
		public static string Unescape(this string text) {
			if (string.IsNullOrEmpty(text)) { return text; }
			var length = text.Length;
			var result = new System.Text.StringBuilder(length);
			for (int i = 0; i < length; ) {
				int j = text.IndexOf('\\', i);
				if (j < 0 || j == length - 1) { j = length; }
				if (j - i > 0) { result.Append(text, i, j - i); }
				if (j >= length) { break; }
				switch (text[j + 1]) {
					case 'n': result.Append('\n'); break;  // Line feed
					case 'r': result.Append('\r'); break;  // Carriage return
					case 't': result.Append('\t'); break;  // Tab
					case '"': result.Append('"'); break;  // Don't escape
					case '\\': result.Append('\\'); break; // Don't escape
					default: result.Append('\\').Append(text[j + 1]); break;
				}
				i = j + 2;
				//UnityEngine.Debug.Log(result.ToString());
			}
			return result.ToString();
		}
	}

}