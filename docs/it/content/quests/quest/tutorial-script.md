---
title: Tutorial (tutorial) - Script
hide:
---

# Tutorial (tutorial) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/quest-tutorial/41)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=631129787#gid=631129787)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/_TUTORIAL/Tutorial%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// tutorial | Tutorial</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Benvenuti al tutorial!</span> <span class="yarn-meta">#line:021793f </span>
<span class="yarn-line">[MISSING TRANSLATION: Here we learn how to play.]</span> <span class="yarn-meta">#line:0588c17 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor-1"></a>

## talk_tutor_1

<div class="yarn-node" data-title="talk_tutor_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tocca questo testo per ascoltarlo nella tua lingua.</span> <span class="yarn-meta">#line:03dbfa7 #native</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_goon&gt;&gt;</span>
<span class="yarn-line">Utilizzare questo pulsante per avanzare nella finestra di dialogo.</span> <span class="yarn-meta">#line:0f4f069 </span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_image&gt;&gt;</span>
<span class="yarn-line">Utilizzare questo pulsante per visualizzare la foto</span> <span class="yarn-meta">#line:0784704 </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Per camminare usa i tasti WASD.</span> <span class="yarn-meta">#line:037d71d </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_move&gt;&gt;</span>
<span class="yarn-line">    Usa il dito sinistro per camminare</span> <span class="yarn-meta">#line:0e55bc4 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Go and talk to next tutor!]</span> <span class="yarn-meta">#line:0eb85aa </span>
<span class="yarn-cmd">&lt;&lt;action area_medium&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-camera"></a>

## tutor_camera

<div class="yarn-node" data-title="tutor_camera">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Premere il tasto destro del mouse per spostare la telecamera.</span> <span class="yarn-meta">#line:0e633a2 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_camera&gt;&gt;</span>
<span class="yarn-line">    Usa il dito destro per muovere la telecamera.</span> <span class="yarn-meta">#line:0aa47cb </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-act"></a>

## tutor_act

<div class="yarn-node" data-title="tutor_act">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_act&gt;&gt;</span>
<span class="yarn-line">Usa questo pulsante per parlare o interagire</span> <span class="yarn-meta">#line:0c14f65 </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Premi il tasto SPAZIO per parlare o interagire.</span> <span class="yarn-meta">#line:0c18f6b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-4-jump"></a>

## tutor_4_jump

<div class="yarn-node" data-title="tutor_4_jump">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: asset=tutorial_move</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Premi il tasto SPAZIO per saltare</span> <span class="yarn-meta">#line:07940cf </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_jump&gt;&gt;</span>
<span class="yarn-line">    Usa questo pulsante per saltare</span> <span class="yarn-meta">#line:0b9c1fa </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_intro_large&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-tutor-5-run"></a>

## tutor_5_run

<div class="yarn-node" data-title="tutor_5_run">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_run&gt;&gt;</span>
<span class="yarn-line">Utilizzare questo pulsante per eseguire.</span> <span class="yarn-meta">#line:093726d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-6-cookies"></a>

## tutor_6_cookies

<div class="yarn-node" data-title="tutor_6_cookies">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cookies&gt;&gt;</span>
<span class="yarn-line">Prendi tutti i biscotti che trovi. Possono essere utili</span> <span class="yarn-meta">#line:0f50a6e </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-7-map"></a>

## tutor_7_map

<div class="yarn-node" data-title="tutor_7_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_map&gt;&gt;</span>
<span class="yarn-line">Questo pulsante apre la mappa!</span> <span class="yarn-meta">#line:01777e4 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-8-interact"></a>

## tutor_8_interact

<div class="yarn-node" data-title="tutor_8_interact">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset  tutorial_actions&gt;&gt;</span>
<span class="yarn-line">Esplora tutti gli oggetti che hanno questa icona.</span> <span class="yarn-meta">#line:0139142 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-9-pushball"></a>

## tutor_9_pushball

<div class="yarn-node" data-title="tutor_9_pushball">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_ball&gt;&gt;</span>
<span class="yarn-line">Prova a spingere questa palla.</span> <span class="yarn-meta">#line:02253ae </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-10-follow"></a>

## tutor_10_follow

<div class="yarn-node" data-title="tutor_10_follow">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_follow&gt;&gt;</span>
<span class="yarn-line">Se ti perdi, segui questa icona.</span> <span class="yarn-meta">#line:06c117d </span>
<span class="yarn-cmd">&lt;&lt;action area_tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-11-mission"></a>

## tutor_11_mission

<div class="yarn-node" data-title="tutor_11_mission">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vediamo se hai imparato: sali le scale e prendi quella moneta!</span> <span class="yarn-meta">#line:0fe9efe </span>
<span class="yarn-cmd">&lt;&lt;action AREA_TUTORIAL&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-teleport"></a>

## tutor_teleport

<div class="yarn-node" data-title="tutor_teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_portal&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Use the portals to travel fast!]</span> <span class="yarn-meta">#line:0f753b5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-killzone"></a>

## tutor_killzone

<div class="yarn-node" data-title="tutor_killzone">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_danger&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Pay attention to not fall into the water!]</span> <span class="yarn-meta">#line:0e9b5a9 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-livingletter"></a>

## tutor_livingletter

<div class="yarn-node" data-title="tutor_livingletter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_livingletter&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: These are Living Letters. Talk to them to learn new words!]</span> <span class="yarn-meta">#line:06e500f </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-blocky-character"></a>

## tutor_blocky_character

<div class="yarn-node" data-title="tutor_blocky_character">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_blocky_character&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: These are our friends. Talk to them to learn more about the world!]</span> <span class="yarn-meta">#line:0be283b </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-card"></a>

## tutor_card

<div class="yarn-node" data-title="tutor_card">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_card&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is a Card. It has knowledge and powers. Collect them all!]</span> <span class="yarn-meta">#line:0ac4c18 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-cat"></a>

## tutor_cat

<div class="yarn-node" data-title="tutor_cat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cat&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Yes. That is you! If you play all the game, you can change your look!]</span> <span class="yarn-meta">#line:0eb1890 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-antura"></a>

## tutor_antura

<div class="yarn-node" data-title="tutor_antura">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: This is Antura, your friend! He will help you in your adventure!]</span> <span class="yarn-meta">#line:015cdc5 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-hallo"></a>

## npc_hallo

<div class="yarn-node" data-title="npc_hallo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ciao!</span> <span class="yarn-meta">#line:0c47c13 #do_not_translate</span>
<span class="yarn-line">Ciao!</span> <span class="yarn-meta">#line:022bd3f #do_not_translate</span>
<span class="yarn-line">Saluti!</span> <span class="yarn-meta">#line:00ad419 #do_not_translate</span>
<span class="yarn-line">чао</span> <span class="yarn-meta">#line:0591baf #do_not_translate</span>

</code>
</pre>
</div>

<a id="ys-node-npc-kid"></a>

## npc_kid

<div class="yarn-node" data-title="npc_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Cookies are hidden all around! Find them!]</span> <span class="yarn-meta">#line:0c8d2fa </span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; I love to use the portals to travel!]</span> <span class="yarn-meta">#line:0fb6855 </span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Talk to everyone you meet!]</span> <span class="yarn-meta">#line:0812c45 </span>
<span class="yarn-line">[MISSING TRANSLATION: =&gt; Explore the world!]</span> <span class="yarn-meta">#line:041d845 </span>

</code>
</pre>
</div>


