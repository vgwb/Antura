# Generating the API documentation

- Download and install [Doxygen](http://www.stack.nl/~dimitri/doxygen/index.html)
- Open Doxygen Wizard
- Configure as follows:
  - Working directory: `/Assets/_app/_scripts`
  - Project name: Antura
  - Source code directory: `/Assets/_app/_scripts`
  - Scan recursively: True
  - Destination directory: Choose an empty `[OutputFolder]`
  - Extraction mode: All Entities
  - Output: HTML
  - Diagrams: built-in
- Run Doxygen
- Copy the contents of `[OutputFolder]\html\` into `/docs/API`
