---
title: Quest Script Writing Guidelines
---

# Quest Script Writing Guidelines

These rules standardize quest Yarn script text for young learners (~6 years old). Apply them BEFORE translating. Keep all technical markers intact.

## 1. Do NOT Touch Technical Elements
- Keep every `#line:HASH` exactly as-is (hash, spacing, position)
- Do not rename nodes, tags, or commands (`<<task_start>>`, `<<card ...>>`, etc.)
- Do not remove or reorder commands around a spoken line
- Only edit the spoken text BEFORE the `#line:` token

## 2. Sentence Shape
| Rule | Target |
|------|--------|
| Words per sentence | 5–12 (max 15 only if colors/list) |
| Tense | Present simple |
| One idea per sentence | Yes |
| Punctuation | Every spoken line ends with `.`, `?`, or `!` |
| Exclamations | Max 1 per short exchange |

## 3. Vocabulary & Tone
- Prefer high-frequency words: help, find, friend, flag, big, small, red, blue
- Allow ONE cultural greeting per country (Bonjour / Hola / Ciao / Danke / Grüezi / Moien). After first use revert to “Hello”
- Keep cultural nouns (Rome, Madrid, flamenco) but simplify surrounding sentence
- Avoid idioms or abstract metaphors

## 4. Flags & Colors
- Keep correct color order (do not improvise)
- Standard pattern: `It has stripes: black, red, yellow.` OR `It is red and white.`
- Limit comparisons: one simple image max (e.g., “like a pizza”)

## 5. Consistent Patterns
| Context | Pattern |
|---------|---------|
| Greeting | `Hello! I'm from COUNTRY!` (or first line with local greeting) |
| Ask help | `Can you help my COUNTRY friend?` |
| Task intro | `Find the COUNTRY flag.` |
| Completion | `Good job!` / `Thank you!` |

## 6. Capitalization & Spelling
- Nationalities & countries capitalized (German, Spanish, Swiss, Luxembourg)
- Colors lowercase (unless start of sentence)
- Fix typos immediately (yellow, Luxembourg)

## 7. Simplification Steps (Apply in Order)
1. Fix typos & capitalization
2. Shorten long sentences (split if > 15 words)
3. Replace rare words / complex verbs
4. Standardize patterns (greeting, help, task, completion)
5. Ensure punctuation
6. Remove redundancy (“the French one” → “my flag”)
7. Final pass: word count & clarity

## 8. What NOT to Change
- Factual information (capital cities, counts, geography)
- Educational objectives or task logic
- Inventory and task progression commands

## 9. When a Line Is Too Complex
| Issue | Fix Example |
|-------|-------------|
| Too many clauses | Split into two lines (if allowed) |
| Abstract phrase | Replace with concrete (“claim your victory” → “get your prize”) |
| Cultural overload | Keep one key detail |

## 10. Placeholders / Missing Translation Handling
- English source should **never** include placeholders

## 11. Quality Checklist (Pre-Commit)
- [ ] All `#line:` tags unchanged
- [ ] Every spoken line has punctuation
- [ ] No long sentence > 15 words
- [ ] No double spaces, no stray leading/trailing spaces
- [ ] Greetings pattern correct
- [ ] Color descriptions concise & accurate
- [ ] No new complex vocabulary slipped in

## 13. Examples
Before:
```
Antura made a mess and all the flags have been mixed up! #line:XXXXXXX
```
After:
```
Antura mixed up all the flags! #line:XXXXXXX
```

Before:
```
Go back to the start and claim your victory! #line:YYYYYYY
```
After:
```
Go back to the start and get your prize! #line:YYYYYYY
```

## 14. Edge Cases
| Case | Guidance |
|------|----------|
| Emotional emphasis | One exclamation OK; avoid stacking (!) |
| Lists > 3 colors | Keep if flag pedagogy requires |
| Foreign word confuses context | Replace with English after first exposure |

## 15. Rationale
These constraints support early readers: predictable syntax, limited working-memory load, reinforcement of factual patterns (flags, capitals), and easy translation alignment.

## version for AI
```
You rewrite child learning game dialogue for 6-year-olds. Keep every #line:HASH tag unchanged at end of its line. Only edit text before the tag. Keep Yarn commands (<< >>) and structure exactly. Rules:

Present simple. 1 idea per sentence. 5–15 words.
End every spoken line with . ? or !
High-frequency words only (help, find, flag, friend, big, small, red, blue). Replace complex words: discover→find, victory→prize.
One cultural greeting per country (Bonjour/Hola/Ciao/Danke/Grüezi/Moien) then use Hello.
Keep factual info (capitals, colors) and order of colors.
Flag colors format: “It has stripes: black, red, yellow.” or “It is red and white.”
Ask help: “Can you help my COUNTRY friend?”
Task: “Find the COUNTRY flag.”
Completion: “Good job!” or “Thank you!”
Capitalize countries/nationalities. Colors lowercase (unless sentence start).
Fix typos. Remove redundancy (“the French one” → “my flag”).
No idioms, no abstract metaphors, no multiple exclamations.
If sentence >12 words, split or simplify.
Do not add new facts.
Checklist before output:

All #line: tags identical
Punctuation present
Each line within word limit
No new complex vocabulary
Color facts correct
Output only the modified script text with original unchanged lines preserved except where simplified.
```
