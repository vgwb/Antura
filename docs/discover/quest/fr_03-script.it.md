---
title: Jules Verne e i trasporti (fr_03) - Script
hide:
---

# Jules Verne e i trasporti (fr_03) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_03-script.md) - [french](./fr_03-script.fr.md) - [polish](./fr_03-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-03-jules-verne-and-transportation/25/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_03%20Nantes%20Verne/FR_03%20Nantes%20Verne%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// FR_03 NANTES_MUSEUM - Jules Verne</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - FIND_BOOKS (collect 4 Jules Verne books)</span>
<span class="yarn-header-dim">// - COLLECT_TRAIN (collect from "Around the World in 80 Days")</span>
<span class="yarn-header-dim">// - COLLECT_ROCKET (collect from "From Earth to the Moon")</span>
<span class="yarn-header-dim">// - COLLECT_SUBMARINE (collect from "20,000 Leagues Under the Sea")</span>
<span class="yarn-header-dim">// - COLLECT_BALLOON (collect from "Five Weeks in a Balloon")</span>
<span class="yarn-header-dim">// Words:</span>
<span class="yarn-header-dim">// writer, map, book, train, rocket, submarine, hot air balloon, science fiction, invention, exploration, adventure</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $QUEST_ITEMS = 4&gt;&gt;</span>
<span class="yarn-line">Welcome to the Museum of Jules Verne in Nantes! <span class="yarn-meta">#line:0b5e2f3</span></span>

</code></pre></div>

<a id="ys-node-talk-guide"></a>
## talk_guide

<div class="yarn-node" data-title="talk_guide"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=CRAZY_WOMAN, asset=jules_verne_1</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $COLLECTED_ITEMS == 0&gt;&gt;</span>
<span class="yarn-line">Welcome to the house of Jules Verne! <span class="yarn-meta">#line:08f7bc1 </span></span>
&lt;&lt;elseif $COLLECTED_ITEMS &lt; $QUEST_ITEMS&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump task_find_books&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump won&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-books"></a>
## task_find_books

<div class="yarn-node" data-title="task_find_books"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BOOKS&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset jverne_mission_overview&gt;&gt;</span>
<span class="yarn-line">Explore the house and find four of his books! <span class="yarn-meta">#line:0aac249 </span></span>


</code></pre></div>

<a id="ys-node-verne-painting"></a>
## verne_painting

<div class="yarn-node" data-title="verne_painting"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=jules_verne_1</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is Jules Verne. He was a writer. <span class="yarn-meta">#line:096a3b3 </span></span>

</code></pre></div>

<a id="ys-node-verne-house"></a>
## verne_house

<div class="yarn-node" data-title="verne_house"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=jules_verne_house</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">He was born in Nantes in 1828. <span class="yarn-meta">#line:003b311 </span></span>

</code></pre></div>

<a id="ys-node-map-nantes"></a>
## map_nantes

<div class="yarn-node" data-title="map_nantes"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR, asset=map_nantes</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is a map of Nantes. <span class="yarn-meta">#line:09bcaba </span></span>

</code></pre></div>

<a id="ys-node-open-chest"></a>
## open_chest

<div class="yarn-node" data-title="open_chest"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 4&gt;&gt;
<span class="yarn-cmd">&lt;&lt;jump won&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump task_find_books&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-won"></a>
## won

<div class="yarn-node" data-title="won"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=CRAZY_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset jules_verne_2&gt;&gt;</span>
<span class="yarn-line">Great! You met Jules Verne, <span class="yarn-meta">#line:099cdca </span></span>
<span class="yarn-line">the famous science fiction writer. <span class="yarn-meta">#line:05a032e </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-book-80days"></a>
## book_80days

<div class="yarn-node" data-title="book_80days"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_80days&gt;&gt;</span>
<span class="yarn-line">This book is "Around the World in 80 Days" <span class="yarn-meta">#line:03131e3</span></span>
<span class="yarn-cmd">&lt;&lt;jump train&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-train"></a>
## train

<div class="yarn-node" data-title="train"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset train&gt;&gt;</span>
<span class="yarn-line">This is an old train <span class="yarn-meta">#line:0732ebc </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_TRAIN&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-moon"></a>
## paint_moon

<div class="yarn-node" data-title="paint_moon"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rocket&gt;&gt;</span>
<span class="yarn-line">This is a space rocket. <span class="yarn-meta">#line:0e5ae78 </span></span>

</code></pre></div>

<a id="ys-node-book-moon"></a>
## book_moon

<div class="yarn-node" data-title="book_moon"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_earthmoon&gt;&gt;</span>
<span class="yarn-line">This book is "From earth to the Moon" <span class="yarn-meta">#line:06df7d0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_moon&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-book-20000legues"></a>
## book_20000legues

<div class="yarn-node" data-title="book_20000legues"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset book_underthesea&gt;&gt;</span>
<span class="yarn-line">This book is 20000 Leagues Under the Seas <span class="yarn-meta">#line:03536a1 </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_20000&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-20000"></a>
## paint_20000

<div class="yarn-node" data-title="paint_20000"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset submarine&gt;&gt;</span>
<span class="yarn-line">This is a submarine <span class="yarn-meta">#line:0f298c2 </span></span>
<span class="yarn-cmd">&lt;&lt;action COLLECT_SUBMARINE&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-paint-5weeks"></a>
## paint_5weeks

<div class="yarn-node" data-title="paint_5weeks"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card balloon&gt;&gt;</span>
<span class="yarn-line">This is a hot air balloon. <span class="yarn-meta">#line:06a7709 </span></span>

</code></pre></div>

<a id="ys-node-book-5weeks"></a>
## book_5weeks

<div class="yarn-node" data-title="book_5weeks"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card book_5weeksballoon&gt;&gt;</span>
<span class="yarn-line">This book is "Five Weeks in a Balloon". <span class="yarn-meta">#line:0934a7c </span></span>
<span class="yarn-cmd">&lt;&lt;jump paint_5weeks&gt;&gt;</span>

</code></pre></div>


