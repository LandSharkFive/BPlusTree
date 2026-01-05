# BPlusTree (Archived / Experimental)

###  Status: Legacy Project
This code is approximately 20 years old and represents an experimental "Hybrid" B+ Tree implementation. 

* **Stability:** Generally stable for small ranges (1 to 40 keys). 
* **Reliability:** Beyond small datasets, there are known bugs in the rebalancing and splitting logic. 
* **Maintenance:** This repository is officially **archived**. I am not seeking contributions or performing further bug fixes. It is preserved here for historical context and code snippets.

---

### Why This Repository is Worth Saving
Despite the bugs in the core balancing logic, there are several "excellent" snippets of code within this project that may be useful to students or developers building their own data structures:

* **Recursive Key Printer:** A robust visualizer for tree structures in the console. Good tree visualization is notoriously difficult to get right, and this implementation remains a strong reference.
* **Search & Print Functions:** High-level, readable implementations of standard B+ Tree traversal logic.

This version uses `List<Node>` for child management. While a production B+ Tree would typically use primitive arrays for memory locality, the `List` approach makes the logic much easier for a human to read and debug.



---

### References
The logic in this implementation was informed by and compared against the following resources:

* **Introduction to Algorithms (CLRS)**, Third Edition, by Cormen, Leiserson, Rivest, and Stein (2009). The foundational math for B-Tree branching.
* **Data Structures the Fun Way** by Jeremy Kubica (2022). An excellent modern resource for visualizing these concepts.
* **B+ Tree (Java Implementation)** by [Shandy Sulen](https://github.com/shandysulen/B-Plus-Tree). 

