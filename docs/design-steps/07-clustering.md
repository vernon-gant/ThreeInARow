### **1. Board Cluster**

Includes classes that manage the structure and operations of the game board. Move, BoardLayout and also EmojiRegistry classes are included in this cluster.

---

### **2. Matching Cluster**

Includes classes that manage the matching of emojis on the game board. This cluster includes classes that manage the matching of emojis on the game board. The `Match` class is used to represent a match of emojis on the board. The `MatchFinder` class is used to find matches on the board. The `MatchHandler` class is used to handle matches on the board.
MatchingStrategy is used to define the matching strategy used by the MatchFinder class.

---

### **3. Statistics Cluster**

Groups all statistics-related classes. We also introduce here a StatisticsUnit class that is used to represent a single statistic. The Statistics class is used to manage all statistics in the game.
Multiple statistics can be added.

---

### **4. Scoring Cluster**

Responsible for scoring rules. Also introduces a single ScoreTracker class that is used to track the score of the player. The Score class is used to represent a single score. The ScoreRule class is used to define a scoring rule.