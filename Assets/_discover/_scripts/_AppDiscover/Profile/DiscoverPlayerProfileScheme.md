## Discover Player Profile JSON scheme

```json
{
  "schemaVersion": 5,
  "metadata": {
    "createdUtc": "2025-08-16T12:00:00Z",
    "lastUpdatedUtc": "2025-08-16T12:00:00Z",
    "platform": "mobile",
    "appVersion": "3.0.0"
  },
  "profile": {
    "id": "player_01",
    "uuid": "5d78f3b0-0c1a-49f2-9c3b-9a3fe1be1e21",
    "displayName": "Alice",
    "locale": "it-IT",
    "countryIso2": "PL",
    "classroom": 2,
    "easyMode": false,
    "talkToPlayerStyle": 3
  },
  "settings": {
    "audio": { "musicMuted": false, "sfxMuted": false }
  },
  "currency": {
    "points": 150,
    "gems": 12,
    "cookies": 4
  },
  "avatar": {
    "activeSpecies": "cat",
    "species": {
      "cat": {
        "prefabId": "AvatarCatV1",
        "equippedProps": [
          "cat/head/hat_chef",
          "cat/eyes/glasses_round"
        ]
      }
    }
  },
  "rewards": {
    "ownedItemIds": [
      "cat/head/hat_chef",
      "cat/eyes/glasses_round"
    ],
    "unlockedItemIds": [
      "cat/body/scarf_blue",
      "cat/head/cap_red"
    ]
  },
  "stats": {
    "totals": {
      "timePlayedSec": 12345,
      "sessions": 57,
      "answersAttempted": 516,
      "answersCorrect": 420
    },
    "quests": {
      "PL_WAW_RIVER": {
        "plays": 8,
        "completions": 6,
        "bestScore": 920,
        "lastScore": 910,
        "sumScore": 6922,
        "timeSec": 1800,
        "bestStars": 3,
        "lastStars": 2,
        "firstPlayedUtc": "2025-08-12T10:20:00Z",
        "lastPlayedUtc": "2025-08-16T11:58:00Z"
      }
    },
    "activities": {
      "Piano01": {
        "plays": 14,
        "wins": 11,
        "bestScore": 100,
        "lastScore": 92,
        "sumScore": 1223,
        "timeSec": 760,
        "didactic": {
          "topic": "notes",
          "attempts": 200,
          "correct": 150,
          "wrongItems": { "note-D": 12 }
        }
      }
    }
  },
  "cards": {
    "CARD_APPLE": {
      "unlocked": true,
      "firstSeenUtc": "2025-08-12T10:22:00Z",
      "lastSeenUtc": "2025-08-16T11:57:00Z",
      "interactions": 23,
      "answered": { "correct": 17, "wrong": 6 },
      "streakCorrect": 4,
      "mastery01": 0.76
    },
    "CARD_FISH": {
      "unlocked": true,
      "firstSeenUtc": "2025-08-13T09:10:00Z",
      "lastSeenUtc": "2025-08-16T11:20:00Z",
      "interactions": 9,
      "answered": { "correct": 6, "wrong": 3 },
      "streakCorrect": 2,
      "mastery01": 0.63
    }
  },
  "achievements": {
    "ACH_FIRST_QUEST": { "unlocked": true,  "progress": 1, "unlockedUtc": "2025-08-12T10:25:00Z" },
    "ACH_10_COMPLETIONS": { "unlocked": false, "progress": 6 },
    "ACH_PIANO_BRONZE":  { "unlocked": true,  "progress": 1, "unlockedUtc": "2025-08-15T09:03:00Z" }
  }
}
```
