==============================================================
Signal Processing Library For .NET
==============================================================

- Author: gbmhunter <gbmhunter@gmail.com> (http://www.cladlab.com)
- Created: 2013/01/13
- Last Modified: 2013/06/04
- Version: v2.0.0.5
- Company: CladLabs
- Project: Free Code Libraries
- Language: C#
- Compiler: .NET	
- uC Model: n/a
- Computer Architecture: .NET
- Operating System: Windows/Linux/MacOS
- Documentation Format: Doxygen
- License: GPLv3

Description
===========

A signal processing library written in C# for the .NET framework.

Internal Dependencies
=====================

Internal dependencies are in "./lib/".

- **MathNet Neodym.** Used for it's FIR filters to create moving averages.
- **ZedGraph.** Graphing library for examples.

External Dependencies
=====================

- None

ISSUES
======

- See GitHub issues section

LIMITATIONS
===========

- None documented

Usage
=====

::

	coming soon...
	
Changelog
=========

======== ========== ===================================================================================================
Version  Date       Comment
======== ========== ===================================================================================================
v2.0.0.5 2013/06/04 Fixed version number in README.
v2.0.0.4 2013/06/04 Formatted README Changelog into table.
v2.0.0.3 2013/06/04 Added ZedGraph and MathNet NeoDym to internal dependencies in README.
v2.0.0.2 2013/06/04 Removed .hgignore and .hgtags files (leftover from Mercurial repo) from root.
v2.0.0.1 2013/06/03 Added README.rst to repo.
v2.0.0.0 2013/03/07 Added WrapAroundDetection algorithm. Tested and works fine.
v1.1.0.0 2013/01/15 Changed the way the extrema detection algorithm works (especially thresholding).
v1.0.1.0 2013/01/13 Split project into source code that generates a DLL and example window forms projects.
v1.0.0.0 2013/01/13 Initial commit. Contains LocalExrema and MovingAverage algorithms.
======== ========== ===================================================================================================
