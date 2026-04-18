\ Generated from forth-standard.org testsuite
\ Wordset: xchar

\ F.18.6.1.2487.15 XC!+? (from testsuite#test:xchar:XC!+q)
T{ $ffff PAD 4 XC!+? -> PAD 3 + 1 <TRUE> }T

\ F.18.6.1.2487.25 XC-SIZE (from testsuite#test:xchar:XC-SIZE)
\ This test assumes UTF-8 encoding is being used.
\ HEX
T{ 0 XC-SIZE -> 1 }T
T{ 7f XC-SIZE -> 1 }T
T{ 80 XC-SIZE -> 2 }T
T{ 7ff XC-SIZE -> 2 }T
T{ 800 XC-SIZE -> 3 }T
T{ ffff XC-SIZE -> 3 }T
T{ 10000 XC-SIZE -> 4 }T
T{ 1fffff XC-SIZE -> 4 }T

\ F.18.6.2.2487.30 XC-WIDTH (from testsuite#test:xchar:XC-WIDTH)
T{ $606D XC-WIDTH -> 2 }T
T{ $41 XC-WIDTH -> 1 }T
T{ $2060 XC-WIDTH -> 0 }T
\ Forth 2012
\ Forth 2012
\ Test Suite
\ Foreword
\ Proposals Process
\ 200x Membership
\ Introduction
\ Terms, notation, and references
\ Usage requirements
\ Documentation requirements
\ Compliance and labeling
\ Glossary
\ Block word set
\ Double-Number word set
\ Exception word set
\ Facility word set
\ File-Access word set
\ Floating-Point word set
\ Locals word set
\ Memory-Allocation word set
\ Programming-Tools word set
\ Search-Order word set
\ String word set
\ Extended-Character word set
\ Rationale
\ Bibliography
\ Compatibility analysis
\ Portability guide
\ Reference Implementations
\ Test Suite
\ Alphabetic list of words
\ Introduction
\ Introduction
\ Test Harness
\ Core Tests
\ The Core word set
\ Block word set
\ Double-Number word set
\ Exception word set
\ Facility word set
\ Facility word set
\ File-Access word set
\ Locals word set
\ Floating-Point word set
\ Programming-Tools word set
\ Memory-Allocation word set
\ String word set
\ Programming-Tools word set
\ Search-Order word set
\ String word set
\ Extended Character word set
\ ContributeContributions
\ StephenPelc
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [63] Let us adopt the Gerry Jackson test suite as part of Forth 200xProposal2018-07-10 14:38:46
\ This contribution has been moved to the proposal section.
\ MarkWills
\ 2018-07-11 08:51:48
\ This reply has been moved to the proposal section.
\ alextangent
\ 2018-07-13 16:25:47
\ This reply has been moved to the proposal section.
\ AntonErtl
\ 2018-07-14 06:32:34
\ This reply has been moved to the proposal section.
\ StephenPelc
\ 2018-07-16 22:45:40
\ This reply has been moved to the proposal section.
\ GeraldWodni
\ 2023-09-14 10:04:02
\ This reply has been moved to the proposal section.
\ Considered
\ Reply New Version
\ JamesNorris
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [145] Please fix word spelling in F.1 second paragraph second word.Suggested reference implementation2020-08-08 10:00:59
\ I believe the author intended to use the word 'test' instead of the word 'teat'.
\ AntonErtl
\ [r563] 2020-09-26 17:10:03
\ Thanks for the report. Will be fixed.
\ Closed
\ Reply New Version
\ MatteoVitturi
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [209] Missed-order in section F.3.10 DivisionExample2021-08-21 14:29:36
\ In section F.3.10 Division, there is a little missed-order in the last sentence:
\ As the test definitions use the words which have just been tested, the tests must be performed in the order: F.6.1.0240 /MOD, F.6.1.0230 /, F.6.1.1890 MOD, F.6.1.0100 */, and F.6.1.0110 */MOD.
\ T*/MOD is referenced in F.6.1.0100 */ but is defined in F.6.1.0110 */MOD, it should be
\ As the test definitions use the words which have just been tested, the tests must be performed in the order: F.6.1.0240 /MOD, F.6.1.0230 /, F.6.1.1890 MOD, F.6.1.0110 */MOD, and F.6.1.0100 */.
\ Matteo
\ Reply New Version
\ LSchmidt
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [222] many tests appear to only assess interpretation semantics of test subjectsSuggested Testcase2022-02-27 18:43:57
\ Tests may be incomplete when only looking at the interpret time semantics. Many Forth systems don't simply compile a call to the same code portion of a tested word. Instead, they may generate code which may be different and unrelated to the executed code during interpretation.
\ Shouldn't those tests therefore not also test against a word which the testee has been compiled to?
\ ruv
\ [r799] 2022-02-28 14:52:55
\ I think, this Test Suite was not intended to cover 100% of possible use cases.
\ In any case, additional tests can be also directly suggested into the forth2012-test-suite repository.
\ Reply New Version
\ LSchmidt
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [223] chasing for dangling words referred toRequest for clarification2022-02-27 20:58:46
\ Looking at test cases for ALLOT, I find <TRUE> and <FALSE>, those defined elsewhere in terms of 0S and 1S. Hunting for those, I find a reference in F.3.2 Booleans, saying:
\ To test the booleans it is first neccessary to test F.6.1.0720 AND, and F.6.1.1720 INVERT. Before moving on to > the test F.6.1.0950 CONSTANT. The latter defines two constants (0S and 1S) which will be used in the further > test.
\ No, it doesn't. it tests the 0S INVERT case to match 1S, and the 1S INVERT case for 0S. Seems that tester is free to come up with his own idea of how to represent 0S and 1S.
\ Reply New Version
\ LSchmidt
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [224] many tests appear to only assess interpretation semantics of test subjectsSuggested Testcase2022-02-27 21:23:05
\ Tests may be incomplete when only looking at the interpret time semantics. Many Forth systems don't simply compile a call to the same code portion of a tested word. Instead, they may generate code which may be different and unrelated to the executed code during interpretation.
\ Shouldn't those tests therefore not also test against a word which the testee has been compiled to?
\ Reply New Version
\ JimPeterson
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [235] F.3 Seems in ErrorComment2022-04-08 17:45:25
\ The end of F.3 states "Note that all of the tests in this suite assume the current base is hexadecimal.", but then there are tests like:
T{ $12eF -> 4847 }T
\ This seems like a contradiction, unless I'm misinterpreting the end of F.3.
\ --Jim
\ ruv
\ [r1141] 2023-12-10 21:27:59
\ Some tests in F.6.1.0140 +LOOP assume the radix is ten, and don't work in hexadecimal.
\ Namely:
T{ -20 31 -10 GD7 -> 31 21 11 1 -9 -19 6 }T
T{ -20 29 -10 GD7 -> 29 19 9 -1 -11 5 }T
\ And the tests that mention the number 256 (literally).
\ Reply New Version
\ EricBlake
\ TODO: make h2 a .header, use badge and colors?
\ TODO: add to user profile
\ TODO: add replies to user profile
\ [381] F.3.18 typoComment2025-06-30 18:50:41
\ "Testing of the input source can be quit dificult." Well yeah, it's well known in computer science that the halting problem is non-computable ;) Please fix the double typo, where it seems like the intended text is "quite difficult".
\ EricBlake
\ [r1455] 2025-07-21 13:54:29
\ Other typos: F.3.1 has "unsinged" instead of "unsigned"; F.3.10 has "signes" instead of "signs". At this point, it is probably better to run a code spell-checker over the entire testsuite than to call out individual problems as I spot them.
\ Reply New Version
\ powered by kern.js
\ a(href="http://wodni.at")
\ ©copyright 2015-2020 Forth-Standard-Committee
