using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolDoc.Editor.Core
{
    public enum Key
    {
		NULL,                                            // NULL
		SOH,                                             // Start of Heading
		STX,                                             // Start of Text
		ETX,                                             // End of Text
		EOT,                                             // End of Transmission
		ENQ,                                             // Enquiry
		ACK,                                             // Acknowledgement
		BEL,                                             // Bell
		BACKSPACE,                                       // Backspace
		TAB,                                             // Horizontal Tab
		LF,                                              // Line Feed 
		VT,                                              // Vertical Tab 
		FF,                                              // Form Feed 
		ENTER,                                           // Carriage Return
		SO,                                              // Shift Out 
		SI,                                              // Shift In 
		DLE,                                             // Data Link Escape 
		DC1,                                             // Device Control 1
		DC2,                                             // Device Control 2
		DC3,                                             // Device Control 3
		DC4,                                             // Device Control 4
		NAK,                                             // Negative Acknowledgement 
		SYN,                                             // Synchronous Idle 
		ETB,                                             // End of Transmission Block 
		CAN,                                             // Cancel
		EM,                                              // End of Medium 
		SUB,                                             // Substitute 
		ESC,                                             // Escape
		SF,                                              // File Separator
		GS,                                              // Group Separator
		RS,                                              // Record Separator
		US,                                              // Unit Separator

		//misc characters				                 

		SPACE,                                           // space
		EXCLAMAION_MARK,                                 // !
		QUOTATION_MARK_DOUBLE,                           // "
		HASHTAG,                                         // #
		SING_DOLLAR,                                     // $
		PRECENT,                                         // %
		AMPERSANT,                                       // &
		QUOTATION_MARK_SINGLE,                           // '
		PARENTHESES_OPEN,                                // (
		PARENTHESES_CLOSE,                               // )
		ASTERISK,                                        // *
		PLUS,                                            // +
		COMMA,                                           // ,
		MINUS,                                           // -
		DOT,                                             // .
		SLASH_FOWARD,                                    // /
		NUMBER_0,                                        // 0
		NUMBER_1,                                        // 1
		NUMBER_2,                                        // 2
		NUMBER_3,                                        // 3
		NUMBER_4,                                        // 4
		NUMBER_5,                                        // 5
		NUMBER_6,                                        // 6
		NUMBER_7,                                        // 7
		NUMBER_8,                                        // 8
		NUMBER_9,                                        // 9
		COLON,                                           // :
		COLON_SEMI,                                      // ;
		LESS_THAN,                                       // <
		EQUAL_TO,                                        // =
		GREATER_THAN,                                    // >
		QUESTION_MARK,                                   // ?
		SING_AT,                                         // @

		//upper case alphabet			                 

		A_UPPER,                                         // A
		B_UPPER,                                         // B
		C_UPPER,                                         // C
		D_UPPER,                                         // D
		E_UPPER,                                         // E
		F_UPPER,                                         // F
		G_UPPER,                                         // G
		H_UPPER,                                         // H
		I_UPPER,                                         // I
		J_UPPER,                                         // J
		K_UPPER,                                         // K
		L_UPPER,                                         // L
		M_UPPER,                                         // M
		N_UPPER,                                         // N
		O_UPPER,                                         // O
		P_UPPER,                                         // P
		Q_UPPER,                                         // Q
		R_UPPER,                                         // R
		S_UPPER,                                         // S
		T_UPPER,                                         // T
		U_UPPER,                                         // U
		V_UPPER,                                         // V
		W_UPPER,                                         // W
		X_UPPER,                                         // X
		Y_UPPER,                                         // Y
		Z_UPPER,                                         // Z

		//misc characters				                 

		BRACKET_OPEN,                                    // [
		SLASH_BACKWARD,                                  // \ 
		BRACKET_CLOSE,                                   // ]
		CARET,                                           // ^
		UNDERSCORE,                                      // _
		GRAVE_ACCENT,                                    // ` 

		//lower case alphabet			                 

		A_LOWER,                                         // a
		B_LOWER,                                         // b
		C_LOWER,                                         // c
		D_LOWER,                                         // d
		E_LOWER,                                         // e
		F_LOWER,                                         // f
		G_LOWER,                                         // g
		H_LOWER,                                         // h
		I_LOWER,                                         // i
		J_LOWER,                                         // j
		K_LOWER,                                         // k
		L_LOWER,                                         // l
		M_LOWER,                                         // m
		N_LOWER,                                         // n
		O_LOWER,                                         // o
		P_LOWER,                                         // p
		Q_LOWER,                                         // q
		R_LOWER,                                         // r
		S_LOWER,                                         // s
		T_LOWER,                                         // t
		U_LOWER,                                         // u
		V_LOWER,                                         // v
		W_LOWER,                                         // w
		X_LOWER,                                         // x
		Y_LOWER,                                         // y
		Z_LOWER,                                         // z

		//misc characters				                 

		BRACKET_CURLY_OPEN,                              // {
		VERTICAL_BAR,                                    // |
		BRACKET_CURLY_CLOSE,                             // }
		TILDE,                                           // ~

		DEL,                                             // Delete

		//===================[extended ASCII]===================\\	                 
		//misc letters					                 

		C_UPPER_CEDILLA,                                 // Ç

		U_LOWER_UMLAUT,                                  // ü
		E_LOWER_ACUTE,                                   // é 
		A_LOWER_CIRCUMFLEX,                              // â
		A_LOWER_UMLAUT,                                  // ä
		A_LOWER_GRAVE,                                   // à
		A_LOWER_RING,                                    // å
		C_LOWER_CEDILLA,                                 // ç
		E_LOWER_CIRCUMFLEX,                              // ê
		E_LOWER_UMLAUT,                                  // ë 
		E_LOWER_GRAVE,                                   // è
		I_LOWER_UMLAUT,                                  // ï 
		I_LOWER_CIRCUMFLEX,                              // î
		I_LOWER_GRAVE,                                   // ì
		A_UPPER_UMLAUT,                                  // Ä
		A_UPPER_RING,                                    // Å
		E_UPPER_ACUTE,                                   // É
		AE_LOWER,                                        // æ 
		AE_UPPER,                                        // Æ 
		O_LOWER_CIRCUMFLEX,                              // ô
		O_LOWER_UMLAUT,                                  // ö
		O_LOWER_GRAVE,                                   // ò
		U_LOWER_CIRCUMFLEX,                              // û
		U_LOWER_GRAVE,                                   // ù
		Y_LOWER_UMLAUT,                                  // ÿ
		O_UPPER_UMLAUT,                                  // Ö
		U_UPPER_UMLAUT,                                  // Ü

		SING_CENT,                                       // ¢
		SING_POUND,                                      // £
		SING_YEN,                                        // ¥
		SING_PESTA,                                      // ₧
		F_LOWER_HOOK,                                    // ƒ

		A_LOWER_ACUTE,                                   // á
		I_LOWER_ACUTE,                                   // í
		O_LOWER_ACUTE,                                   // ó
		U_LOWER_ACUTE,                                   // ú
		N_LOWER_TILDE,                                   // ñ
		N_UPPER_TILDE,                                   // Ñ

		//symbols						                 

		ORDINAL_INDICATOR_FEMININE,                      // ª
		ORDINAL_INDICATOR_MASCULINE,                     // º
		QUESTION_MARK_REVERSED,                          // ¿
		SING_NOT_REVERSED,                               // ⌐
		SING_NOT,                                        // ¬
		VULGAR_FRACTION_HALF,                            // ½
		VULGAR_FRACTION_QUARTER,                         // ¼
		EXCLAMATION_MARK_INVERTED,                       // ¡
		QUOTATION_MARK_DOUBLE_ANGLE_LEFT,                // «
		QUOTATION_MARK_DOUBLE_ANGLE_RIGHT,               // »

		/* box drawings
		* BOX_[direction1]_[thickness]_[direction2]_[thickness]
		* if you cant find a combination try switching the direction combination
		*/

		SHADE_LIGHT,                                     // ░
		SHADE_MEDIUM,                                    // ▒
		SHADE_DARK,                                      // ▓

		BOX_VERTICAL_LIGHT,                              // │
		BOX_VERTICAL_LIGHT_LEFT_LIGHT,                   // ┤
		BOX_VERTICAL_SINGLE_LEFT_DOUBLE,                 // ╡
		BOX_VERTICAL_DOUBLE_LEFT_SINGLE,                 // ╢
		BOX_DOWN_DOUBLE_LEFT_SINGLE,                     // ╖
		BOX_DOWN_SINGLE_LEFT_DOUBLE,                     // ╕
		BOX_VERTICAL_DOUBLE_LEFT_DOUBLE,                 // ╣
		BOX_VERTICAL_DOUBLE,                             // ║
		BOX_DOWN_DOUBLE_LEFT_DOUBLE,                     // ╗
		BOX_UP_DOUBLE_LEFT_DOUBLE,                       // ╝
		BOX_UP_DOUBLE_LEFT_SINGLE,                       // ╜
		BOX_UP_SINGLE_LEFT_DOUBLE,                       // ╛
		BOX_DOWN_LIGHT_LEFT_LIGHT,                       // ┐
		BOX_UP_LIGHT_RIGHT_LIGHT,                        // └
		BOX_UP_LIGHT_HORIZONTAL_LIGHT,                   // ┴
		BOX_DOWN_LIGHT_HORIZONTAL_LIGHT,                 // ┬
		BOX_VERTICAL_LIGHT_RIGHT_LIGHT,                  // ├
		BOX_HORIZONTAL_LIGHT,                            // ─
		BOX_VERTICAL_LIGHT_HORIZONTAL_LIGHT,             // ┼
		BOX_VERTICAL_SINGLE_RIGHT_DOUBLE,                // ╞
		BOX_VERTICAL_DOUBLE_RIGHT_SINGLE,                // ╟
		BOX_UP_DOUBLE_RIGHT_DOUBLE,                      // ╚
		BOX_DOWN_DOUBLE_RIGHT_DOUBLE,                    // ╔
		BOX_UP_DOUBLE_HORIZONTAL_DOUBLE,                 // ╩
		BOX_DOWN_DOUBLE_HORIZONTAL_DOUBLE,               // ╦
		BOX_VERTICAL_DOUBLE_RIGHT_DOUBLE,                // ╠
		BOX_HORIZONTAL_DOUBLE,                           // ═
		BOX_VERTICAL_DOUBLE_HORIZONTAL_DOUBLE,           // ╬
		BOX_UP_SINGLE_HORIZONTAL_DOUBLE,                 // ╧
		BOX_UP_DOUBLE_HORIZONTAL_SINGLE,                 // ╨
		BOX_DOWN_SINGLE_HORIZONTAL_DOUBLE,               // ╤
		BOX_DOWN_DOUBLE_HORIZONTAL_SINGLE,               // ╥
		BOX_UP_DOUBLE_RIGHT_SINGLE,                      // ╙
		BOX_UP_SINGLE_RIGHT_SINGLE,                      // ╘
		BOX_DOWN_SINGLE_RIGHT_DOUBLE,                    // ╒
		BOX_DOWN_SINGLE_RIGHT_SINGLE,                    // ╓
		BOX_VETRICAL_DOUBLE_HORIZONTAL_SINGLE,           // ╫
		BOX_VERTICAL_SINGLE_HORIZONTALDOUBLE,            // ╪
		BOX_UP_LIGHT_LEFT_LIGHT,                         // ┘
		BOX_DOWN_LIGHT_RIGHT_LIGHT,                      // ┌

		BLOCK_FULL,                                      // █
		BLOCK_HALF_LOWER,                                // ▄
		BLOCK_HALF_LEFT,                                 // ▌
		BLOCK_HALF_RIGHT,                                // ▐
		BLOCK_HALF_UPPER,                                // ▀

		//other letters

		APLHA_LOWER,                                     // α
		S_SHARP_LOWER,                                   // ß
		GAMMA_UPPER,                                     // Γ
		PI_LOWER,                                        // π
		SIGMA_UPPER,                                     // Σ
		SIGMA_LOWER,                                     // σ

		SING_MICRO,                                      // µ
		TAU_LOWER,                                       // τ
		PHI_UPPER,                                       // Φ
		THETA_UPPER,                                     // Θ
		OMEGA_UPPER,                                     // Ω
		DELTA_LOWER,                                     // δ
		INFINITY,                                        // ∞
		PHI_LOWER,                                       // φ
		EPSILON_LOWER,                                   // ε
		INTERSECTION,                                    // ∩
		IDENTICAL_TO,                                    // ≡
		SING_PLUS_MINUS,                                 // ±
		GREATER_THAN_OR_EQUAL_TO,                        // ≥
		LESS_THAN_OR_EQUAL_TO,                           // ≤
		HALF_INTEGRAL_TOP,                               // ⌠
		HALF_INTEGRAL_BOTTOM,                            // ⌡
		SING_DIVISION,                                   // ÷
		EQUAL_TO_ALMOST,                                 // ≈
		SING_DEGREE,                                     // °
		BULLER_OPERATOR,                                 // ∙
		DOT_MIDDLE,                                      // ·
		SQUARE_ROOT,                                     // √
		N_SUPERSCRIPT_LOWER,                             // ⁿ
		NUMBER_2_SUPERSCRIPT,                            // ²
		BLACK_SQUARE,                                    // ■
		SPACE_NO_BREAK,                                  //

		// Special keys
		INSERT,
		HOME,
		END,
		PAGE_UP,
		PAGE_DOWN,
		ARROW_RIGHT,
		ARROW_LEFT,
		ARROW_UP,
		ARROW_DOWN,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		PRINT_SCREEN,
		BREAK
	}
}
