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
| Words per sentence | 5–12 |
| Tense | Present simple |
| One idea per sentence | Yes |
| Punctuation | Every spoken line ends with `.`, `?`, or `!` |
| Exclamations | Max 1 per short exchange |

## 3. Vocabulary & Tone
- Prefer high-frequency words: help, find, friend, flag, big, small, red, blue
- Allow ONE cultural greeting per country (Bonjour / Hola / Ciao / Danke / Grüezi / Moien). After first use revert to “Hello”
- Keep cultural nouns (Rome, Madrid, flamenco) but simplify surrounding sentence
- Avoid idioms or abstract metaphors

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


## 15. Rationale
These constraints support early readers: predictable syntax, limited working-memory load, reinforcement of factual patterns, and easy translation alignment.
