---
title: Wielka akcja ratunkowa krasnala wrocławskiego (pl_02) - Script
hide:
---

# Wielka akcja ratunkowa krasnala wrocławskiego (pl_02) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-02-the-great-wroclaw-dwarf-rescue/33/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1721014062#gid=1721014062)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_02%20Wroclaw%20Dwarves/PL_02%20Wroclaw%20Dwarves%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_02 | Dwarves (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $current_place = "school"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_0_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_1_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_2_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_3_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_4_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_5_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_6_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_7_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_8_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_9_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_10_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $area_center_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $area_centennial_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $secret_0_done = false&gt;&gt;</span>

<span class="yarn-comment">// hide all the keys</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_0 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_2 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_3 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_4 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_5 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_6 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_7 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_8 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_9 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_10 false&gt;&gt;</span>

<span class="yarn-comment">// DEBUG (comment in production)</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_0_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_1_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_2_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_3_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_4_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_5_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_6_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_7_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_8_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_9_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;set $dwarf_10_done = true&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;jump tram&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card wroclaw&gt;&gt;</span>
<span class="yarn-line">Witamy we Wrocławiu!</span> <span class="yarn-meta">#line:082706f </span>
<span class="yarn-line">W mieście pojawił się problem... zbadajmy go!</span> <span class="yarn-meta">#line:029d26c </span>
<span class="yarn-cmd">&lt;&lt;jump intro_area_school&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ukończyłeś to zadanie. Podobało Ci się?</span> <span class="yarn-meta">#line:0069d94 </span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj swojego ulubionego wrocławskiego krasnala i miejsce, w którym go znalazłeś.</span> <span class="yarn-meta">#line:00d7df1 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-teleport"></a>

## teleport

<div class="yarn-node" data-title="teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $area_center_done and $area_centennial_done&gt;&gt;</span>
<span class="yarn-line">    Teraz możesz udać się do Sky Tower!</span> <span class="yarn-meta">#line:042161c</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Dokąd chcesz iść?</span> <span class="yarn-meta">#line:where_to_go</span>
-&gt; School <span class="yarn-cmd">&lt;&lt;if $current_place == "cathedral"&gt;&gt;</span>  <span class="yarn-meta">#line:0d0e53a </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_school&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_school&gt;&gt;</span>
-&gt; Cathedral <span class="yarn-cmd">&lt;&lt;if $current_place != "cathedral"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_cathedral&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_cathedral&gt;&gt;</span>
-&gt; Market Hall <span class="yarn-cmd">&lt;&lt;if $current_place != "center"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_market</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_center&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_center&gt;&gt;</span>
-&gt; Panorama <span class="yarn-cmd">&lt;&lt;if $current_place == "center" and !$dwarf_4_done&gt;&gt;</span>  <span class="yarn-meta">#line:01008e2 </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_panorama&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_panorama&gt;&gt;</span>
-&gt; Centennial Hall <span class="yarn-cmd">&lt;&lt;if $current_place != "centennial"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_centennial</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_centennial_hall&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_centennial&gt;&gt;</span>
-&gt; Sky Tower <span class="yarn-cmd">&lt;&lt;if $area_centennial_done and $area_center_done and $current_place != "skytower"&gt;&gt;</span>  <span class="yarn-meta">#line:012413f </span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_skytower&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_skytower&gt;&gt;</span>
<span class="yarn-choice">-&gt; Zostań tutaj</span> <span class="yarn-meta">#line:stay_here #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-school"></a>

## driver_tram_school

<div class="yarn-node" data-title="driver_tram_school">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">Dokąd chcesz iść?</span> <span class="yarn-meta">#shadow:where_to_go</span>
-&gt; Cathedral <span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>  <span class="yarn-meta">#shadow:tram_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_cathedral&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_cathedral&gt;&gt;</span>
<span class="yarn-choice">-&gt; Zostań tutaj</span> <span class="yarn-meta">#shadow:stay_here #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Najpierw wykonaj zadanie!</span> <span class="yarn-meta">#line:0beae23 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-cathedral"></a>

## driver_tram_cathedral

<div class="yarn-node" data-title="driver_tram_cathedral">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-center"></a>

## driver_tram_center

<div class="yarn-node" data-title="driver_tram_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-panorama"></a>

## driver_tram_panorama

<div class="yarn-node" data-title="driver_tram_panorama">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dokąd chcesz iść?</span> <span class="yarn-meta">#shadow:where_to_go</span>
<span class="yarn-choice">-&gt; Hala targowa</span> <span class="yarn-meta">#shadow:tram_market</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_center&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_center&gt;&gt;</span>
<span class="yarn-choice">-&gt; Hala Stulecia</span> <span class="yarn-meta">#shadow:tram_centennial</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_centennial_hall&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_centennial&gt;&gt;</span>
<span class="yarn-choice">-&gt; Zostań tutaj</span> <span class="yarn-meta">#shadow:stay_here #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-centennial"></a>

## driver_tram_centennial

<div class="yarn-node" data-title="driver_tram_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-driver-tram-skytower"></a>

## driver_tram_skytower

<div class="yarn-node" data-title="driver_tram_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump teleport&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-school"></a>

## intro_area_school

<div class="yarn-node" data-title="intro_area_school">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA SCHOOL</span>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "school"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Jesteśmy w Szkole Podstawowej Leonardo!</span> <span class="yarn-meta">#line:005d84c </span>

</code>
</pre>
</div>

<a id="ys-node-npc-0"></a>

## npc_0

<div class="yarn-node" data-title="npc_0">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">    Teraz wiesz, jak uwolnić pozostałe krasnoludy.</span> <span class="yarn-meta">#line:04b5c6e </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_skytower_roof&gt;&gt;</span>
<span class="yarn-line">    Antura na szczycie Sky Tower!</span> <span class="yarn-meta">#line:09771a2 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Tramwaj jest gotowy. Najpierw jedź do Katedry.</span> <span class="yarn-meta">#line:00f4741 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_0"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card primary_school_leonardo_da_vinci&gt;&gt;</span>
<span class="yarn-line">    Cześć! Jestem nauczycielem ze szkoły Leonardo.</span> <span class="yarn-meta">#line:08d54cc</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_town_view&gt;&gt;</span>
<span class="yarn-line">    Antura obiegła dziś cały Wrocław!</span> <span class="yarn-meta">#line:0b6daaa</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_dwarfs&gt;&gt;</span>
<span class="yarn-line">    A dziś rano dziesięciu krasnoludków zostało zamkniętych w klatkach!</span> <span class="yarn-meta">#line:0b42c98</span>
<span class="yarn-line">    Każda klatka jest zamknięta na złoty klucz.</span> <span class="yarn-meta">#line:084b7e8 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_school&gt;&gt;</span>
<span class="yarn-line">    Ratuj szkolnego krasnoludka.</span> <span class="yarn-meta">#line:0294a8c </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#line:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_0 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-0"></a>

## info_0

<div class="yarn-node" data-title="info_0">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card primary_school_leonardo_da_vinci&gt;&gt;</span>
<span class="yarn-line">Czy lubisz chodzić do szkoły?</span> <span class="yarn-meta">#line:info_00</span>
<span class="yarn-choice">-&gt; Tak</span> <span class="yarn-meta">#shadow:yes</span>
    <span class="yarn-cmd">&lt;&lt;if !$secret_0_done&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $secret_0_done = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;collect cookie&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-choice">-&gt; NIE</span> <span class="yarn-meta">#shadow:no</span>

</code>
</pre>
</div>

<a id="ys-node-hint-0"></a>

## hint_0

<div class="yarn-node" data-title="hint_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#line:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#line:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#line:055d8b9 </span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#line:where_key </span>
<span class="yarn-line">    Przesuń roślinę, aby ją znaleźć!</span> <span class="yarn-meta">#line:0a5ff98 </span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#line:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_0 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#line:02da44a </span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#line:goodbye </span>

</code>
</pre>
</div>

<a id="ys-node-key-0"></a>

## key_0

<div class="yarn-node" data-title="key_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś! Teraz idź i odblokuj krasnoluda!</span> <span class="yarn-meta">#line:0289364 </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-0"></a>

## dwarf_0

<div class="yarn-node" data-title="dwarf_0">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_expert&gt;&gt;</span>
<span class="yarn-line">    Dawno temu zbudowałem w mieście Tunele Krasnoludów.</span> <span class="yarn-meta">#line:0e45f38 </span>
    <span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">    Teraz zamieniły się w magiczne pojazdy zwane „tramwajami”.</span> <span class="yarn-meta">#line:cf_014b</span>
<span class="yarn-line">    Daję Ci ten specjalny bilet, dzięki któremu będziesz mógł podróżować wszędzie!</span> <span class="yarn-meta">#line:0c46606 </span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_map&gt;&gt;</span>
<span class="yarn-line">    A teraz idź i uwolnij wszystkich moich 10 krasnoludzkich przyjaciół.</span> <span class="yarn-meta">#line:0b401f5 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_cathedral_overview&gt;&gt;</span>
<span class="yarn-line">    Najpierw udaj się do Katedry.</span> <span class="yarn-meta">#line:0816b22 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_school&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#line:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_0 activity_0_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#line:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-0-done"></a>

## activity_0_done

<div class="yarn-node" data-title="activity_0_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory tram add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_0 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_0 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_0_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#line:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#line:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-0"></a>

## chest_0

<div class="yarn-node" data-title="chest_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: school</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_0_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_0_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_0 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-cathedral"></a>

## intro_area_cathedral

<div class="yarn-node" data-title="intro_area_cathedral">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CATHEDRAL</span>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "cathedral"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_cathedral&gt;&gt;</span>
<span class="yarn-line">Jesteśmy na przystanku Bema, niedaleko Katedry.</span> <span class="yarn-meta">#line:0d7ac4b </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_1_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-1"></a>

## npc_1

<div class="yarn-node" data-title="npc_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
<span class="yarn-line">    Wybierz następny przystanek tramwajem.</span> <span class="yarn-meta">#line:03991b3 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_cathedral&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_1"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_bishop&gt;&gt;</span>
<span class="yarn-line">    Biskup Krasnolud jest zamknięty w pobliżu.</span> <span class="yarn-meta">#line:0769945 </span>
<span class="yarn-line">    Znajdź klucz do katedry i uwolnij go!</span> <span class="yarn-meta">#line:0ebb3bc </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-1"></a>

## info_1

<div class="yarn-node" data-title="info_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_cathedral&gt;&gt;</span>
<span class="yarn-line">To jest Katedra Wrocławska.</span> <span class="yarn-meta">#line:0eb59b5</span>
<span class="yarn-line">Katedra znajduje się w jednej z najstarszych części Wrocławia.</span> <span class="yarn-meta">#line:info_1a</span>
<span class="yarn-line">Ludzie przychodzą tutaj, żeby się pomodlić i w ciszy pomyśleć.</span> <span class="yarn-meta">#line:info_1b</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_cathedral&gt;&gt;</span>
<span class="yarn-line">Ma wysokie wieże i piękne okna.</span> <span class="yarn-meta">#line:07f68f0 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-hint-1"></a>

## hint_1

<div class="yarn-node" data-title="hint_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;target ll_key_1&gt;&gt;</span>
<span class="yarn-line">    Ktoś w pobliżu to ma.</span> <span class="yarn-meta">#line:0166015</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_1 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1"></a>

## ll_key_1

<div class="yarn-node" data-title="ll_key_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "dwarf_1"&gt;&gt;</span>
<span class="yarn-line">Co robią ludzie w katedrze?</span> <span class="yarn-meta">#line:0e6869e </span>
<span class="yarn-choice">-&gt; Oni się modlą</span> <span class="yarn-meta">#line:0a1aab0 </span>
<span class="yarn-line">    Dobrze! Oto Twój klucz.</span> <span class="yarn-meta">#line:correct_key </span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_1 true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target key_1&gt;&gt;</span>
<span class="yarn-choice">-&gt; Oni czytają</span> <span class="yarn-meta">#line:0e25e89 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#line:try_again</span>
<span class="yarn-choice">-&gt; Oni grają</span> <span class="yarn-meta">#line:0340188 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump info_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-1"></a>

## key_1

<div class="yarn-node" data-title="key_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś! Teraz idź i odblokuj krasnoluda!</span> <span class="yarn-meta">#shadow:0289364 </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-1"></a>

## dwarf_1

<div class="yarn-node" data-title="dwarf_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_bishop&gt;&gt;</span>
<span class="yarn-line">    Zostaję tutaj, aby pomóc ludziom odnaleźć spokój.</span> <span class="yarn-meta">#line:08ae2a5 </span>
<span class="yarn-line">    Moi przyjaciele nadal potrzebują pomocy w Centennial Hall i centrum miasta.</span> <span class="yarn-meta">#line:0b7a174 </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_cathedral&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_1 activity_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-1-done"></a>

## activity_1_done

<div class="yarn-node" data-title="activity_1_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_map add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_1 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_1 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_1_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;card wroclaw_map&gt;&gt;</span>
<span class="yarn-line">    Rysowałem mapę Wrocławia.</span> <span class="yarn-meta">#line:0fcb25a </span>
<span class="yarn-line">    Użyj go, aby zobaczyć, gdzie jesteś i dokąd iść!</span> <span class="yarn-meta">#line:0d493c7</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-1"></a>

## chest_1

<div class="yarn-node" data-title="chest_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cathedral</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_1_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-center"></a>

## intro_area_center

<div class="yarn-node" data-title="intro_area_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CENTER</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "center"&gt;&gt;</span>
<span class="yarn-line">Spójrz na Rynek Główny, Ratusz i Panoramę Racławicką!</span> <span class="yarn-meta">#line:0ccc1a3 </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-check-area-center"></a>

## check_area_center

<div class="yarn-node" data-title="check_area_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_2_done&gt;&gt;</span>
<span class="yarn-line">    Krasnolud Origin jest na targu.</span> <span class="yarn-meta">#line:08a9541 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_3_done&gt;&gt;</span>
<span class="yarn-line">    Krasnoludek Radny znajduje się w pobliżu Starego Ratusza.</span> <span class="yarn-meta">#line:councilor_near_old_town_hall</span>
    <span class="yarn-cmd">&lt;&lt;target npc_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_4_done&gt;&gt;</span>
<span class="yarn-line">    Ostatnim miejscem tutaj jest Panorama Racławicka.</span> <span class="yarn-meta">#line:panorama_last_place_here</span>
    <span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">    Jeśli nie chcesz iść pieszo, możesz skorzystać z tramwaju.</span> <span class="yarn-meta">#line:use_tram</span>
    <span class="yarn-cmd">&lt;&lt;target npc_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Strefa centralna gotowa!</span> <span class="yarn-meta">#line:center_area_done</span>
    <span class="yarn-cmd">&lt;&lt;set $area_center_done = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-2"></a>

## npc_2

<div class="yarn-node" data-title="npc_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_2"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_2&gt;&gt;</span>
<span class="yarn-line">    Wrocław słynie z małych krasnoludków.</span> <span class="yarn-meta">#line:05b17f6 </span>
<span class="yarn-line">    Mają malutkie kapelusze i malutkie buty!</span> <span class="yarn-meta">#line:0f0ca08 </span>
<span class="yarn-line">    Uwolnij Krasnoluda Początkowego.</span> <span class="yarn-meta">#line:04fde74 </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_2 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-2"></a>

## info_2

<div class="yarn-node" data-title="info_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_market_square&gt;&gt;</span>
<span class="yarn-line">Rynek Główny jest jednym z najbardziej ruchliwych miejsc we Wrocławiu.</span> <span class="yarn-meta">#line:info_2a</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarf_statue&gt;&gt;</span>
<span class="yarn-line">Podczas spacerów wiele osób rozgląda się za figurkami krasnoludków!</span> <span class="yarn-meta">#line:info_2b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-2"></a>

## hint_2

<div class="yarn-node" data-title="hint_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Klucz znajduje się pod doniczką na targu.</span> <span class="yarn-meta">#line:0d121cc</span>
    <span class="yarn-cmd">&lt;&lt;target key_2&gt;&gt;</span>
<span class="yarn-line">    Jeśli nie znajdziesz ich wszystkich, skorzystaj z mapy!</span> <span class="yarn-meta">#line:use_map_find</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_2 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-2"></a>

## key_2

<div class="yarn-node" data-title="key_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#line:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-2"></a>

## dwarf_2

<div class="yarn-node" data-title="dwarf_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_origin&gt;&gt;</span>
<span class="yarn-line">    Jestem jednym z najstarszych krasnoludów w mieście! Po raz pierwszy pojawiłem się w 2005 roku.</span> <span class="yarn-meta">#line:025f12a</span>
<span class="yarn-line">    W mieście jest ponad 800 krasnoludów!</span> <span class="yarn-meta">#line:08150b1 </span>
<span class="yarn-line">    Czy znajdziesz nas wszystkich?</span> <span class="yarn-meta">#line:09ff330 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_2 activity_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-2-done"></a>

## activity_2_done

<div class="yarn-node" data-title="activity_2_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_2 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_2 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_2_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-2"></a>

## chest_2

<div class="yarn-node" data-title="chest_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_done&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;inventory key_gold remove&gt;&gt;</span>
<span class="yarn-line">	Skrzynia się otwiera!</span> <span class="yarn-meta">#line:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_2_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_2 false&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;SetActive dwarf_2 true&gt;&gt;</span>
<span class="yarn-comment">	// &lt;&lt;SetMapIcon chest_2 off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#line:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-3"></a>

## npc_3

<div class="yarn-node" data-title="npc_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_3"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_3") &gt;&gt;</span>
<span class="yarn-line">    Idź do Krasnoluda z kluczem!</span> <span class="yarn-meta">#line:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_3&gt;&gt;</span>
<span class="yarn-line">    Tutaj, w Starym Ratuszu, przestrzegamy praw miasta.</span> <span class="yarn-meta">#line:0de0a2f </span>
<span class="yarn-line">    Ale nasz radny Krasnolud zgubił wszystkie swoje książki!</span> <span class="yarn-meta">#line:04f7a17 </span>
<span class="yarn-line">    Znajdź wszystkie książki o krasnoludach!</span> <span class="yarn-meta">#line:02ba9a9 </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_3 task_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-3-done"></a>

## task_3_done

<div class="yarn-node" data-title="task_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś je wszystkie! Wróć po nagrodę.</span> <span class="yarn-meta">#line:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_3 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-3"></a>

## info_3

<div class="yarn-node" data-title="info_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card town_hall&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_old_town_hall&gt;&gt;</span>
<span class="yarn-line">Stary Ratusz to stary i ważny budynek.</span> <span class="yarn-meta">#line:info_3a</span>
<span class="yarn-line">Zbudowano go około 1300 roku!</span> <span class="yarn-meta">#line:info_3b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-3"></a>

## hint_3

<div class="yarn-node" data-title="hint_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Zbierz wszystkie książki.</span> <span class="yarn-meta">#line:0d450fd </span>
<span class="yarn-line">    Jeśli nie znajdziesz ich wszystkich, skorzystaj z mapy!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_3 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-3"></a>

## key_3

<div class="yarn-node" data-title="key_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#line:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-3"></a>

## dwarf_3

<div class="yarn-node" data-title="dwarf_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_councilor&gt;&gt;</span>
<span class="yarn-line">    Jestem Radnym Krasnoludem. Pomagam Burmistrzowi Miasta!</span> <span class="yarn-meta">#line:0851c6f </span>
<span class="yarn-line">    Moja rodzina mieszka tu od bardzo, bardzo dawna!</span> <span class="yarn-meta">#line:06002b8 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_3 activity_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-3-done"></a>

## activity_3_done

<div class="yarn-node" data-title="activity_3_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_3 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_3 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_3_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-3"></a>

## chest_3

<div class="yarn-node" data-title="chest_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_3_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_3 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-panorama"></a>

## intro_area_panorama

<div class="yarn-node" data-title="intro_area_panorama">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;area area_panorama&gt;&gt;</span>
<span class="yarn-line">Panorama Racławicka to gigantyczny, okrągły obraz!</span> <span class="yarn-meta">#line:0fb27ac </span>
<span class="yarn-line">To obraz, który Cię otacza.</span> <span class="yarn-meta">#line:016bb86 </span>
<span class="yarn-line">Znajdź Krasnoluda Malarza tutaj.</span> <span class="yarn-meta">#line:0c878db </span>

</code>
</pre>
</div>

<a id="ys-node-npc-4"></a>

## npc_4

<div class="yarn-node" data-title="npc_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_center&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_4"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_4") &gt;&gt;</span>
<span class="yarn-line">    Idź do Krasnoluda z kluczem!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_4&gt;&gt;</span>
<span class="yarn-line">    Nasz krasnoludek malarz stracił wszystkie swoje kolory!</span> <span class="yarn-meta">#line:07177c7 </span>
<span class="yarn-line">    Znajdź wszystkie kolory!</span> <span class="yarn-meta">#line:0737a2f</span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_4 task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-4-done"></a>

## task_4_done

<div class="yarn-node" data-title="task_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś je wszystkie! Wróć po nagrodę.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_4 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-4"></a>

## info_4

<div class="yarn-node" data-title="info_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card panorama_racawicka&gt;&gt;</span>
<span class="yarn-line">Panorama Racławicka to ogromny obraz, który wypełnia całe pomieszczenie!</span> <span class="yarn-meta">#line:info_4a</span>
<span class="yarn-line">Dzięki temu poczujesz się, jakbyś był wewnątrz obrazu!</span> <span class="yarn-meta">#line:info_4b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-4"></a>

## hint_4

<div class="yarn-node" data-title="hint_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Zbierz wszystkie kolory w parku.</span> <span class="yarn-meta">#line:key_under_the_frame</span>
<span class="yarn-line">    Jeśli nie znajdziesz ich wszystkich, skorzystaj z mapy!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_4 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-4"></a>

## key_4

<div class="yarn-node" data-title="key_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-4"></a>

## dwarf_4

<div class="yarn-node" data-title="dwarf_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_painter&gt;&gt;</span>
<span class="yarn-line">    Maluję historię Wrocławia, aby każdy mógł ją zobaczyć.</span> <span class="yarn-meta">#line:dwarf4_003</span>
<span class="yarn-line">    Uwielbiam jaskrawe kolory i wielkie obrazy!</span> <span class="yarn-meta">#line:dwarf4_004</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_4 activity_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-4-done"></a>

## activity_4_done

<div class="yarn-node" data-title="activity_4_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_4 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_4 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_4_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-4"></a>

## chest_4

<div class="yarn-node" data-title="chest_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: panorama</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_4_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_4 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-centennial"></a>

## intro_area_centennial

<div class="yarn-node" data-title="intro_area_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CENTENNIAL HALL</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "centennial"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_centennial&gt;&gt;</span>
<span class="yarn-line">Witamy w Centennial Hall!</span> <span class="yarn-meta">#line:0db2417 </span>
<span class="yarn-line">Zajrzyj do Zoo, do Wielkiej Hali i Fontanny.</span> <span class="yarn-meta">#line:05bd1c1 </span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-check-area-centennial"></a>

## check_area_centennial

<div class="yarn-node" data-title="check_area_centennial">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_5_done&gt;&gt;</span>
<span class="yarn-line">    W Zoo jest co robić!</span> <span class="yarn-meta">#line:0b13c52 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_6_done&gt;&gt;</span>
<span class="yarn-line">    Czy sprawdzałeś w środku Centennial Hall?</span> <span class="yarn-meta">#line:095f631 </span>
    <span class="yarn-cmd">&lt;&lt;target npc_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_7_done&gt;&gt;</span>
<span class="yarn-line">    Fontanna Multimedialna potrzebuje naszej pomocy!</span> <span class="yarn-meta">#line:0bc156b </span>
    <span class="yarn-cmd">&lt;&lt;target npc_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $area_centennial_done = true&gt;&gt;</span>
<span class="yarn-line">    Teren Hali Stulecia jest gotowy!</span> <span class="yarn-meta">#line:0d209ba </span>
    <span class="yarn-cmd">&lt;&lt;target driver_tram_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-5"></a>

## npc_5

<div class="yarn-node" data-title="npc_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// ZOO</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_5"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_5") &gt;&gt;</span>
<span class="yarn-line">    Idź do Krasnoluda z kluczem!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_5&gt;&gt;</span>
<span class="yarn-line">    Nasz krasnoludek, miłośnik zwierząt, kocha wszystkie zwierzęta</span> <span class="yarn-meta">#line:048c7ab </span>
<span class="yarn-line">    Ale uciekli, gdy pojawił się wielki niebieski pies!</span> <span class="yarn-meta">#line:05deab1 </span>
<span class="yarn-line">    Znajdź wszystkie zwierzęta i uwolnij krasnoluda!</span> <span class="yarn-meta">#line:09b7b40 </span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_5 task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-5-done"></a>

## task_5_done

<div class="yarn-node" data-title="task_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś je wszystkie! Wróć po nagrodę.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_5 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-5"></a>

## info_5

<div class="yarn-node" data-title="info_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarfs&gt;&gt;</span>
<span class="yarn-line">Wrocławskie Zoo jest największym ogrodem zoologicznym w Polsce!</span> <span class="yarn-meta">#line:07c9455</span>
<span class="yarn-line">Ogród zoologiczny został zbudowany dawno temu, w 1865 roku!</span> <span class="yarn-meta">#line:info_5a</span>
<span class="yarn-line">Rodziny przyjeżdżają tutaj, aby zobaczyć zwierzęta z całego świata.</span> <span class="yarn-meta">#line:info_5b</span>

</code>
</pre>
</div>

<a id="ys-node-ll-spawn-5"></a>

## ll_spawn_5

<div class="yarn-node" data-title="ll_spawn_5">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Lubię zoo!</span> <span class="yarn-meta">#line:08a038b </span>
<span class="yarn-line">Chcę chronić wszystkie zwierzęta</span> <span class="yarn-meta">#line:09033f1 </span>
<span class="yarn-line">Gdzie jest mój kot?</span> <span class="yarn-meta">#line:01dc29f </span>

</code>
</pre>
</div>

<a id="ys-node-hint-5"></a>

## hint_5

<div class="yarn-node" data-title="hint_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Znajdź wszystkie zwierzęta w zoo.</span> <span class="yarn-meta">#line:0d0946a </span>
<span class="yarn-line">    Jeśli nie znajdziesz ich wszystkich, skorzystaj z mapy!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_5 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-5"></a>

## key_5

<div class="yarn-node" data-title="key_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-5"></a>

## dwarf_5

<div class="yarn-node" data-title="dwarf_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_animal_lover&gt;&gt;</span>
<span class="yarn-line">    Opiekuję się zwierzętami. Ty też kochasz zwierzęta?</span> <span class="yarn-meta">#line:dwarf5_001</span>
<span class="yarn-line">    Mam nadzieję, że będziesz się dobrze bawić w zoo!</span> <span class="yarn-meta">#line:dwarf5_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_5 activity_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-5-done"></a>

## activity_5_done

<div class="yarn-node" data-title="activity_5_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_5&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_5 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_5 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_5_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-5"></a>

## chest_5

<div class="yarn-node" data-title="chest_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_5_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_5 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-6"></a>

## npc_6

<div class="yarn-node" data-title="npc_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// CENTENNIAL HALL</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_6"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_6&gt;&gt;</span>
<span class="yarn-line">    Pomocy! Nasz Krasnolud Architekt zniknął!</span> <span class="yarn-meta">#line:0c3a081 </span>
<span class="yarn-line">    Potrzebujemy go do ulepszenia budynków</span> <span class="yarn-meta">#line:088d0bd </span>
<span class="yarn-line">    Znajdź i uratuj Krasnoluda Architekta!</span> <span class="yarn-meta">#line:092e472</span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_6&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_6 true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable iglica_lever true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-6"></a>

## info_6

<div class="yarn-node" data-title="info_6">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">Hala Stulecia ma gigantyczny, okrągły dach.</span> <span class="yarn-meta">#line:info_6a</span>
<span class="yarn-line">To jeden z najsłynniejszych budynków w mieście!</span> <span class="yarn-meta">#line:info_6b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-6"></a>

## hint_6

<div class="yarn-node" data-title="hint_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf </span>
<span class="yarn-line">    Znajdź klucz. Potem otwórz klatkę!</span> <span class="yarn-meta">#shadow:055d8b9</span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial_iglica&gt;&gt;</span>
<span class="yarn-line">    Widzę! Na szczycie Iglicy!</span> <span class="yarn-meta">#line:0abe6d9</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial&gt;&gt;</span>
<span class="yarn-line">    Ktoś widział, jak wchodził na dach</span> <span class="yarn-meta">#line:0a7bbd6 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_6 on&gt;&gt;</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-lever-6"></a>

## lever_6

<div class="yarn-node" data-title="lever_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_centennial_iglica&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;wait 3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_6&gt;&gt;</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>

</code>
</pre>
</div>

<a id="ys-node-key-6"></a>

## key_6

<div class="yarn-node" data-title="key_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_6&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-6"></a>

## dwarf_6

<div class="yarn-node" data-title="dwarf_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_architect&gt;&gt;</span>
<span class="yarn-line">    Uwielbiam dachy i wysokie łuki!</span> <span class="yarn-meta">#line:dwarf6_001</span>
<span class="yarn-line">    Zawsze myślę o nowych budynkach do narysowania.</span> <span class="yarn-meta">#line:dwarf6_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_6 activity_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-6-done"></a>

## activity_6_done

<div class="yarn-node" data-title="activity_6_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_6&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_6 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_6 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_6_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-6"></a>

## chest_6

<div class="yarn-node" data-title="chest_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_6_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_6 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-7"></a>

## npc_7

<div class="yarn-node" data-title="npc_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// MULTIMEDIA FOUNTAIN</span>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_7"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_7") &gt;&gt;</span>
<span class="yarn-line">    Idź do Krasnoluda z kluczem!</span> <span class="yarn-meta">#shadow:go_dwarf_key</span>
    <span class="yarn-cmd">&lt;&lt;target dwarf_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_7&gt;&gt;</span>
<span class="yarn-line">    Nasz Krasnal prowadził pokazy fontann.</span> <span class="yarn-meta">#line:01d8fa1 </span>
<span class="yarn-line">    Ale teraz zgubił wszystkie notatki!</span> <span class="yarn-meta">#line:0f05296 </span>
<span class="yarn-line">    Znajdź muzykę i uratuj krasnoluda-dyrygenta!</span> <span class="yarn-meta">#line:041a696 </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_7 task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-7-done"></a>

## task_7_done

<div class="yarn-node" data-title="task_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś je wszystkie! Wróć po nagrodę.</span> <span class="yarn-meta">#shadow:collected_all</span>
<span class="yarn-cmd">&lt;&lt;SetActive key_7 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_7&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-7"></a>

## info_7

<div class="yarn-node" data-title="info_7">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_multimedia_fountain&gt;&gt;</span>
<span class="yarn-line">Ta fontanna wykorzystuje wodę, kolorowe światła i muzykę!</span> <span class="yarn-meta">#line:info_7a </span>
<span class="yarn-line">Jest to program multimedialny, ponieważ wykorzystuje wiele różnych rzeczy.</span> <span class="yarn-meta">#line:info_7b</span>
<span class="yarn-line">Przyjdźcie tu wieczorem i zobaczcie to widowisko!</span> <span class="yarn-meta">#line:info_7c</span>

</code>
</pre>
</div>

<a id="ys-node-hint-7"></a>

## hint_7

<div class="yarn-node" data-title="hint_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf</span>
    <span class="yarn-cmd">&lt;&lt;card musical_scale&gt;&gt;</span>
<span class="yarn-line">    Zagraj nuty we właściwej kolejności.</span> <span class="yarn-meta">#line:0ddb6b5 </span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
<span class="yarn-line">    Znajdź wszystkie nuty muzyczne w tym miejscu</span> <span class="yarn-meta">#line:0d919ae </span>
<span class="yarn-line">    Jeśli nie znajdziesz ich wszystkich, skorzystaj z mapy!</span> <span class="yarn-meta">#shadow:use_map_find</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_7 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-7"></a>

## key_7

<div class="yarn-node" data-title="key_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_7&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-7"></a>

## dwarf_7

<div class="yarn-node" data-title="dwarf_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_conductor&gt;&gt;</span>
<span class="yarn-line">    Pokaz fontann przypomina wielki koncert!</span> <span class="yarn-meta">#line:dwarf7_002</span>
    <span class="yarn-cmd">&lt;&lt;card piano&gt;&gt;</span>
<span class="yarn-line">    Czy grasz na jakimś instrumencie muzycznym?</span> <span class="yarn-meta">#line:dwarf7_001</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_centennial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card musical_scale&gt;&gt;</span>
<span class="yarn-line">    Zagraj nuty, aby mnie odblokować!</span> <span class="yarn-meta">#line:0622e7b </span>
    <span class="yarn-cmd">&lt;&lt;activity activity_7 activity_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-7-done"></a>

## activity_7_done

<div class="yarn-node" data-title="activity_7_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_7&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_7 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_7 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_7_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-7"></a>

## chest_7

<div class="yarn-node" data-title="chest_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_7_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_7 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-skytower"></a>

## intro_area_skytower

<div class="yarn-node" data-title="intro_area_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA SKY TOWER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "skytower"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_skytower&gt;&gt;</span>
<span class="yarn-line">Witamy w Sky Tower! Jest bardzo wysoki.</span> <span class="yarn-meta">#line:05da79c </span>

</code>
</pre>
</div>

<a id="ys-node-check-area-skytower"></a>

## check_area_skytower

<div class="yarn-node" data-title="check_area_skytower">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_8_done&gt;&gt;</span>
<span class="yarn-line">    Pisarz Krasnolud jest nadal zablokowany!</span> <span class="yarn-meta">#line:0eb3a6c </span>
    <span class="yarn-cmd">&lt;&lt;target npc_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_9_done&gt;&gt;</span>
<span class="yarn-line">    Podróżujący Krasnolud jest na placu.</span> <span class="yarn-meta">#line:06183de </span>
    <span class="yarn-cmd">&lt;&lt;target npc_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">    Klucznik jest uwięziony w pobliżu windy!</span> <span class="yarn-meta">#line:0977e6d </span>
    <span class="yarn-cmd">&lt;&lt;target npc_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Wszystkie krasnoludy ze Sky Tower są wolne!</span> <span class="yarn-meta">#line:08a24f3 </span>
<span class="yarn-line">    Winda jest już otwarta.</span> <span class="yarn-meta">#line:0c540ac </span>
    <span class="yarn-cmd">&lt;&lt;target ll_elevator&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-8"></a>

## npc_8

<div class="yarn-node" data-title="npc_8">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// WRITER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_8"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_8&gt;&gt;</span>
<span class="yarn-line">    Pomóż krasnoludowi pisarzowi.</span> <span class="yarn-meta">#line:0ac8824 </span>
<span class="yarn-line">    Uwielbia opowieści, książki i... biblioteki.</span> <span class="yarn-meta">#line:082386e </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_8&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_8 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-8"></a>

## info_8

<div class="yarn-node" data-title="info_8">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card olga_tokarczuk&gt;&gt;</span>
<span class="yarn-line">W pobliżu mieszkała słynna pisarka Olga Tokarczuk!</span> <span class="yarn-meta">#line:0474fb4 </span>
<span class="yarn-line">Ona dostała wielką nagrodę za swoje książki. Nagrodę Nobla!</span> <span class="yarn-meta">#line:info_8a</span>
<span class="yarn-line">Może możesz poczytać książkę w bibliotece?</span> <span class="yarn-meta">#line:info_8b</span>

</code>
</pre>
</div>

<a id="ys-node-hint-8"></a>

## hint_8

<div class="yarn-node" data-title="hint_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Klucz znajduje się w kąciku czytelniczym!</span> <span class="yarn-meta">#line:0d82e66 </span>
<span class="yarn-cmd">&lt;&lt;target key_8&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-8"></a>

## key_8

<div class="yarn-node" data-title="key_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_8&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-8"></a>

## dwarf_8

<div class="yarn-node" data-title="dwarf_8">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_writer&gt;&gt;</span>
<span class="yarn-line">    Uwielbiam trzymać w rękach dobrą książkę!</span> <span class="yarn-meta">#line:dwarf8_001</span>
<span class="yarn-line">    Czasami siedzę tu i wyobrażam sobie swoje własne historie.</span> <span class="yarn-meta">#line:dwarf8_003</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_8 activity_8_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-8-done"></a>

## activity_8_done

<div class="yarn-node" data-title="activity_8_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_8&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_8 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_8 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_8_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-8"></a>

## chest_8

<div class="yarn-node" data-title="chest_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_8_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_8 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-9"></a>

## npc_9

<div class="yarn-node" data-title="npc_9">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// TRAVELER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_9"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;detour info_9&gt;&gt;</span>
<span class="yarn-line">    Nasz Krasnolud Podróżnik uwielbia zwiedzać nowe miejsca.</span> <span class="yarn-meta">#line:info_017</span>
<span class="yarn-line">    Ale zgubił się w labiryncie.</span> <span class="yarn-meta">#line:02172f2</span>
<span class="yarn-line">    Znajdź i uratuj Krasnoludzkiego Podróżnika!</span> <span class="yarn-meta">#line:07cb84f </span>
<span class="yarn-line">    Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start dwarf_9&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive key_9 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-9"></a>

## info_9

<div class="yarn-node" data-title="info_9">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sky_tower_plaza&gt;&gt;</span>
<span class="yarn-line">Plac to duże, otwarte miejsce, gdzie spotykają się ludzie.</span> <span class="yarn-meta">#line:info_9a </span>
<span class="yarn-cmd">&lt;&lt;card dali_profile_of_time&gt;&gt;</span>
<span class="yarn-line">Znajduje się tutaj słynna rzeźba autorstwa artysty o imieniu Salvador Dalí!</span> <span class="yarn-meta">#line:info_9c</span>

</code>
</pre>
</div>

<a id="ys-node-hint-9"></a>

## hint_9

<div class="yarn-node" data-title="hint_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jak mogę pomóc?</span> <span class="yarn-meta">#shadow:how_can_help </span>
<span class="yarn-choice">-&gt; Jak mogę uwolnić Krasnoluda?</span> <span class="yarn-meta">#shadow:how_free_dwarf</span>
<span class="yarn-line">   Połóż klucz na książkach.</span> <span class="yarn-meta">#line:07430fb </span>
<span class="yarn-choice">-&gt; Gdzie jest klucz?</span> <span class="yarn-meta">#shadow:where_key</span>
    <span class="yarn-cmd">&lt;&lt;target key_9&gt;&gt;</span>
<span class="yarn-line">    Aby go zdobyć, przeskakuj przez półki.</span> <span class="yarn-meta">#line:0acc3e1</span>
<span class="yarn-choice">-&gt; Gdzie jest krasnolud?</span> <span class="yarn-meta">#shadow:where_dwarf </span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_9 on&gt;&gt;</span>
<span class="yarn-line">    Spójrz na gwiazdkę na mapie.</span> <span class="yarn-meta">#shadow:02da44a</span>
<span class="yarn-choice">-&gt; Do widzenia</span> <span class="yarn-meta">#shadow:goodbye</span>

</code>
</pre>
</div>

<a id="ys-node-key-9"></a>

## key_9

<div class="yarn-node" data-title="key_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_9&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-9"></a>

## dwarf_9

<div class="yarn-node" data-title="dwarf_9">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_traveler&gt;&gt;</span>
<span class="yarn-line">    Uwielbiam patrzeć na tę rzeźbę Salvadora Dalí!</span> <span class="yarn-meta">#line:dwarf9_001</span>
<span class="yarn-line">    Chciałbym kiedyś podróżować dookoła świata.</span> <span class="yarn-meta">#line:dwarf9_002</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("key_gold")&gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_9 activity_9_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-9-done"></a>

## activity_9_done

<div class="yarn-node" data-title="activity_9_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;task_end dwarf_9&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_9 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_9 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_map_distance -1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_9_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-9"></a>

## chest_9

<div class="yarn-node" data-title="chest_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_9_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_9 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-10"></a>

## npc_10

<div class="yarn-node" data-title="npc_10">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// KEYMASTER</span>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "dwarf_10"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump hint_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $dwarf_8_done and $dwarf_9_done&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;detour info_10&gt;&gt;</span>
<span class="yarn-line">        Runda finałowa!</span> <span class="yarn-meta">#line:0e6a977 </span>
<span class="yarn-line">        Klucznik potrzebuje 10 Kluczy Krasnoludów!</span> <span class="yarn-meta">#line:0e72fd8</span>
<span class="yarn-line">        Skontaktuj się ze mną, jeśli potrzebujesz pomocy.</span> <span class="yarn-meta">#shadow:talk_4_help</span>
        <span class="yarn-cmd">&lt;&lt;camera_map_distance 30&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_start dwarf_10 task_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Najpierw odblokuj pozostałych dwóch krasnoludów, a potem wróć.</span> <span class="yarn-meta">#line:0a8844b </span>
        <span class="yarn-cmd">&lt;&lt;if !$dwarf_8_done&gt;&gt;</span>
            <span class="yarn-cmd">&lt;&lt;target npc_8&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
            <span class="yarn-cmd">&lt;&lt;target npc_9&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-10-done"></a>

## task_10_done

<div class="yarn-node" data-title="task_10_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: centennial</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Idź do Mistrza Krasnoludów!</span> <span class="yarn-meta">#line:06bc72f </span>
<span class="yarn-cmd">&lt;&lt;target dwarf_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-10"></a>

## info_10

<div class="yarn-node" data-title="info_10">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_sky_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_skytower_roof&gt;&gt;</span>
<span class="yarn-line">Sky Tower jest jednym z najwyższych budynków we Wrocławiu.</span> <span class="yarn-meta">#line:info_10a</span>
<span class="yarn-line">Ze szczytu można zobaczyć niemal całe miasto!</span> <span class="yarn-meta">#line:info_10b</span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-hint-10"></a>

## hint_10

<div class="yarn-node" data-title="hint_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ten krasnolud potrzebuje ostatniego klucza, aby być wolnym!</span> <span class="yarn-meta">#line:031d093 </span>
<span class="yarn-cmd">&lt;&lt;SetActive key_10 true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target key_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-key-10"></a>

## key_10

<div class="yarn-node" data-title="key_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oto Twój klucz!</span> <span class="yarn-meta">#shadow:a_key</span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>
<span class="yarn-line">Teraz możesz odblokować krasnoluda!</span> <span class="yarn-meta">#shadow:unlock_dwarf</span>
<span class="yarn-cmd">&lt;&lt;target dwarf_10&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-10"></a>

## dwarf_10

<div class="yarn-node" data-title="dwarf_10">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card dwarf_keymaster&gt;&gt;</span>
<span class="yarn-line">    W końcu znów mogę pilnować wszystkich zamków!</span> <span class="yarn-meta">#line:dwarf10_001</span>
<span class="yarn-line">    Odblokuję ci windę.</span> <span class="yarn-meta">#line:0c6f241</span>
    <span class="yarn-cmd">&lt;&lt;jump check_area_skytower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("dwarf_10") &gt;&gt;</span>
<span class="yarn-line">    Aby otworzyć klatkę, rozwiąż tę zagadkę!</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_10 activity_10_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Jest zamknięte. Znajdź złoty klucz!</span> <span class="yarn-meta">#shadow:locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-10-done"></a>

## activity_10_done

<div class="yarn-node" data-title="activity_10_done">
<pre class="yarn-code" style="--node-color:---"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">color:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;inventory key_gold remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarf_statue add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive cage_10 false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetMapIcon dwarf_10 done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $dwarf_10_done = true&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Uratowałeś mnie!</span> <span class="yarn-meta">#shadow:dwarf_saved</span>
    <span class="yarn-cmd">&lt;&lt;jump dwarf_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Spróbuj ponownie później!</span> <span class="yarn-meta">#shadow:try_again_later</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-10"></a>

## chest_10

<div class="yarn-node" data-title="chest_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: skytower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera!</span> <span class="yarn-meta">#shadow:chest_opens</span>
    <span class="yarn-cmd">&lt;&lt;trigger chest_10_open&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable chest_10 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-elevator"></a>

## ll_elevator

<div class="yarn-node" data-title="ll_elevator">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// SKY TOWER TOP AND FINALE</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_done&gt;&gt;</span>
<span class="yarn-line">Chcesz wejść na dach?</span> <span class="yarn-meta">#line:088ccf1 </span>
<span class="yarn-choice">-&gt; Tak</span> <span class="yarn-meta">#line:yes</span>
<span class="yarn-line">    Wchodzimy na szczyt Sky Tower!</span> <span class="yarn-meta">#line:0bca140</span>
    <span class="yarn-cmd">&lt;&lt;SetInteractable elevatore_level true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;trigger elevator&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-choice">-&gt; NIE</span> <span class="yarn-meta">#line:no </span>
<span class="yarn-line">    Wróć, kiedy będziesz gotowy.</span> <span class="yarn-meta">#line:0b51799</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Winda nie działa!</span> <span class="yarn-meta">#line:0a94351 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-roof"></a>

## npc_roof

<div class="yarn-node" data-title="npc_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Miasto stąd wygląda przepięknie!</span> <span class="yarn-meta">#line:035a519</span>
<span class="yarn-line">I spójrz... Antura też tu jest!</span> <span class="yarn-meta">#line:05950e6 </span>
<span class="yarn-line">Czy chcesz odpowiedzieć na kilka pytań, aby ukończyć to zadanie?</span> <span class="yarn-meta">#line:071d502 </span>
<span class="yarn-choice">-&gt; Tak</span> <span class="yarn-meta">#shadow:yes</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-choice">-&gt; Nie, chcę tu zostać</span> <span class="yarn-meta">#line:021c568 </span>
<span class="yarn-line">    Wróć, kiedy będziesz gotowy.</span> <span class="yarn-meta">#shadow:0b51799 </span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q1"></a>

## assessment_q1

<div class="yarn-node" data-title="assessment_q1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jaki jest słynny symbol Wrocławia?</span> <span class="yarn-meta">#line:0509f87 </span>
<span class="yarn-choice">-&gt; Fontanna</span> <span class="yarn-meta">#line:0ad5b67 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-choice">-&gt; Krasnolud</span> <span class="yarn-meta">#line:0b73e95 </span>
<span class="yarn-line">    Tak, to prawda!</span> <span class="yarn-meta">#line:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-choice">-&gt; Klucz</span> <span class="yarn-meta">#line:0c5d850 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q2"></a>

## assessment_q2

<div class="yarn-node" data-title="assessment_q2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co robią ludzie w katedrze?</span> <span class="yarn-meta">#line:07cc31d </span>
<span class="yarn-choice">-&gt; Oni się modlą</span> <span class="yarn-meta">#line:0416812 </span>
<span class="yarn-line">    Tak, to prawda!</span> <span class="yarn-meta">#shadow:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Oni czytają</span> <span class="yarn-meta">#line:0bab60b </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-choice">-&gt; Oni grają</span> <span class="yarn-meta">#line:0509c68 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q3"></a>

## assessment_q3

<div class="yarn-node" data-title="assessment_q3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Kim jest Olga Tokarczuk?</span> <span class="yarn-meta">#line:0041cc7 </span>
<span class="yarn-choice">-&gt; Polityk</span> <span class="yarn-meta">#line:0aa7047 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Naukowiec</span> <span class="yarn-meta">#line:0c4a0b5 </span>
<span class="yarn-line">    Nie. Spróbuj ponownie!</span> <span class="yarn-meta">#shadow:try_again</span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_q3&gt;&gt;</span>
<span class="yarn-choice">-&gt; Pisarz</span> <span class="yarn-meta">#line:09368b4 </span>
<span class="yarn-line">    Tak, to prawda!</span> <span class="yarn-meta">#shadow:yes_right </span>
    <span class="yarn-cmd">&lt;&lt;jump assessment_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-end"></a>

## assessment_end

<div class="yarn-node" data-title="assessment_end">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota! Zwiedziłeś Wrocław i uwolniłeś wszystkich krasnoludków.</span> <span class="yarn-meta">#line:0cedfa4 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-arcade"></a>

## arcade

<div class="yarn-node" data-title="arcade">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chcesz zagrać w grę?</span> <span class="yarn-meta">#line:08e8a18 </span>
<span class="yarn-choice">-&gt; Pamięć</span> <span class="yarn-meta">#line:game_memory </span>
    <span class="yarn-cmd">&lt;&lt;activity activity_9 arcade normal&gt;&gt;</span>
<span class="yarn-choice">-&gt; Puzzle</span> <span class="yarn-meta">#line:game_jigsaw</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_6 arcade normal&gt;&gt;</span>
<span class="yarn-choice">-&gt; NIE</span> <span class="yarn-meta">#shadow:no</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Panorama wokół mnie wygląda jak gigantyczny obraz!</span> <span class="yarn-meta">#line:055554f </span>
<span class="yarn-line">Wiele osób odwiedza Wrocław w poszukiwaniu figur krasnali.</span> <span class="yarn-meta">#line:0d17afb </span>
<span class="yarn-line">Hala Stulecia słynie ze swojego dużego, okrągłego dachu.</span> <span class="yarn-meta">#line:005b996 </span>
<span class="yarn-line">Fontanna multimedialna wykorzystuje wodę, światło i muzykę.</span> <span class="yarn-meta">#line:07d11df </span>
<span class="yarn-line">Sky Tower jest jednym z najwyższych budynków we Wrocławiu.</span> <span class="yarn-meta">#shadow:info_10a </span>
<span class="yarn-line">Rynek Główny to bardzo ruchliwe i przyjemne miejsce.</span> <span class="yarn-meta">#line:0173c2b </span>
<span class="yarn-line">Niektóre wrocławskie krasnale wyglądają zabawnie, a niektóre poważnie!</span> <span class="yarn-meta">#line:0805d72 </span>

</code>
</pre>
</div>


