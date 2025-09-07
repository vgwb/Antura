---
title: La Marsigliese (fr_11) - Script
hide:
---

# La Marsigliese (fr_11) - Script
[Quest Index](./index.it.md) - Language: [english](./fr_11-script.md) - [french](./fr_11-script.fr.md) - [polish](./fr_11-script.pl.md) - italian

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-11-la-marseillaise/30/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=849141304#gid=849141304)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_11%20Music%20Marseillese/FR_11%20Music%20Marseillese%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code"><code><span class="yarn-header-dim">// Quest: fr_11 | La Marseillaise</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome to the music quest!] <span class="yarn-meta">#line:0e2f565 </span></span>
<span class="yarn-comment">// &lt;&lt;activity order_marseillese_audio marseillese_played&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: This quest is over.]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Can you write the tezt of the Marseillaise in your notebook?]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-npc-robot"></a>
## npc_robot

<div class="yarn-node" data-title="npc_robot"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: now play Jigsaw] <span class="yarn-meta">#line:09d7993 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_marseillese marseillese_played&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-npc-ll"></a>
## npc_ll

<div class="yarn-node" data-title="npc_ll"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: now play Order] <span class="yarn-meta">#line:00f2510 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_marseillese_audio marseillese_played&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-marseillese-played"></a>
## marseillese_played

<div class="yarn-node" data-title="marseillese_played"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Well done! You played "La Marseillaise"!] <span class="yarn-meta">#line:0a5f3e1</span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-band-member"></a>
## band_member

<div class="yarn-node" data-title="band_member"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 7&gt;&gt;
<span class="yarn-line">[MISSING TRANSLATION: Thank you for helping us!] <span class="yarn-meta">#line:09fc7ad </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Can you put it together?] <span class="yarn-meta">#line:0515a95 </span></span>
<span class="yarn-cmd">&lt;&lt;action order_notes&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello! We're part of a band. We're musicians.] <span class="yarn-meta">#line:09a50f8 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: We wanted to play the French Anthem, but we cannot!] <span class="yarn-meta">#line:0a9df26 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Antura has mixed up the musical script.] <span class="yarn-meta">#line:0f0ccdf </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Find the notes of the script.] <span class="yarn-meta">#line:0c9616b </span></span>
<span class="yarn-cmd">&lt;&lt;task_start find_the_script_parts&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-jean-michelle-jarre"></a>
## jean_michelle_jarre

<div class="yarn-node" data-title="jean_michelle_jarre"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COLLECTED_ITEMS &gt;= 11&gt;&gt;
<span class="yarn-line">[MISSING TRANSLATION: Thank you! The words of the French Hymn are important.] <span class="yarn-meta">#line:025e35b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: The French National Anthem, "La Marseillaise" rapresents France and its people.] <span class="yarn-meta">#line:0e4484d </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Can you put the words in order?] <span class="yarn-meta">#line:07e4ff1 </span></span>
<span class="yarn-cmd">&lt;&lt;activity order_words&gt;&gt;</span>
&lt;&lt;elseif $COLLECTED_ITEMS &gt;= 7&gt;&gt;
<span class="yarn-line">[MISSING TRANSLATION: Hello I'm Jean Michelle Jarre.] <span class="yarn-meta">#line:02f7c8b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: I'm a french composer, and I'm helping the band play "La Marseillaise".] <span class="yarn-meta">#line:0bd77b7 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Find the words of the anthem.] <span class="yarn-meta">#line:0e7033c </span></span>
<span class="yarn-line">[MISSING TRANSLATION: They've been scattered around by Antura.] <span class="yarn-meta">#line:0dc84d4 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start find_the_words&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: I'm busy right now, come talk to me later] <span class="yarn-meta">#line:08be987 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-win-order"></a>
## win_order

<div class="yarn-node" data-title="win_order"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Now let's try to play the song!] <span class="yarn-meta">#line:0cef358 </span></span>
<span class="yarn-cmd">&lt;&lt;activity play_piano&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>


