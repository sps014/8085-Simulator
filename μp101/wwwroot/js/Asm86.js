CodeMirror.defineMode("asm86", function () {
	var keywords1 = /^(mov|mvi|add|adi|hlt|cma|sub|sui|adc|aci|sbb|sbi|lda|sta|jnz|inr|jnc|jc|jz)\b/i;
	var keywords2 = /^(hlt|HLT)\b/i;
	var keywords3 = /^(e?[abcd]x|[abcd][lh]|e?(si|di|bp|sp)|eip)\b/i;
	var keywords4 = /^(d?word|byte|ptr)\b/i;
	var numbers = /^(0x[0-9a-f]+|0b[01]+|[0-9]+|[0-9][0-9a-f]+h|[0-1]+b)\b/i;
	return {
		startState: function () {
			return { context: 0 };
		},
		token: function (stream, state) {
			//if (!stream.column())
			//	state.context = 0;
			if (stream.eatSpace())
				return null;
			var w;
			if (stream.eatWhile(/\w/)) {
				w = stream.current();
				if (keywords1.test(w)) {
					//state.context = 1;
					return "keyword";
				} else if (keywords2.test(w)) {
					//state.context = 2;
					return "keyword-2";
				} else if (keywords3.test(w)) {
					//state.context = 3;
					return "keyword-3";
				} else if (keywords4.test(w)) {
					return "operator";
				} else if (numbers.test(w)) {
					return "number";
				} else {
					return null;
				}
			} else if (stream.eat(";")) {
				stream.skipToEnd();
				return "comment";
			} else if (stream.eat(",") || stream.eat(".") || stream.eat(":") || stream.eat("[") || stream.eat("]") || stream.eat("+") || stream.eat("-") || stream.eat("*")) {
				return "operator";
			} else {
				stream.next();
			}
			return null;
		}
	};
});
CodeMirror.defineMIME("text/plain", "txt");
