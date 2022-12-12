# Lack of Cohesion Of Methods (LCOM) Analyzer for Roslyn

First attempt to compute the LCOM 1 of an implementation type. The analyzer triggers a diagnostic warning whenever a `class`, `struct` and body-defined `record` has of Lcom greater than 0.8.