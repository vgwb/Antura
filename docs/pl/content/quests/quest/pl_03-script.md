---
title: Rzeka Odra (pl_03) - Script
hide:
---

# Rzeka Odra (pl_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_03 | Odra River (Wroclaw)</span>
<span class="yarn-header-dim">// PL_03 - A Voyage on the Odra River</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 8&gt;&gt;</span>
<span class="yarn-comment">// State variables for the 8 chests</span>
<span class="yarn-cmd">&lt;&lt;declare $map_odra = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $river_sign = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_tumski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_redzinski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_train = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_house = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_tourist = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_barge = 0&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">Jesteśmy we Wrocławiu, „Mieście Stu Mostów”.</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-line">Dzisiaj będziemy zwiedzać rzekę, mosty i łodzie.</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze zrobiony!</span> <span class="yarn-meta">#line:02a257c </span>
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">Poznaliśmy różne rodzaje mostów.</span> <span class="yarn-meta">#line:end_1</span>
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">Poznaliśmy różne rodzaje łodzi.</span> <span class="yarn-meta">#line:end_2</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">Eksplorowaliśmy ODRĘ.</span> <span class="yarn-meta">#line:end_3</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Narysuj prostą MAPĘ swojej podróży.</span> <span class="yarn-meta">#line:0265d07 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-protagonist"></a>

## protagonist

<div class="yarn-node" data-title="protagonist">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_photos")&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Znalazłeś wszystkie moje zdjęcia!</span> <span class="yarn-meta">#line:prot_1</span>
<span class="yarn-line">    Teraz mogę kontynuować wizytę. Jeszcze jedno ostatnie pytanie...</span> <span class="yarn-meta">#line:prot_2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    Jesteś odważny! Ale wciąż tęsknię za zdjęciami naszych rzecznych cudów.</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Szukaj skrzyń! Ludzie nad rzeką ich pilnują.</span> <span class="yarn-meta">#line:prot_4 </span>
<span class="yarn-line">    Jeśli nie możesz ich znaleźć, skorzystaj z mapy!</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    Dobrze ci idzie!</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Pomocy! Antura myślała, że ​​mój przewodnik po Wrocławiu to kość!</span> <span class="yarn-meta">#line:prot_5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    Strony są gdzieś zagubione. Czy potrafisz je znaleźć?</span> <span class="yarn-meta">#line:prot_6 #task:collect_cards</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_tutorial&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_cards task_collect_cards_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-photos-done"></a>

## task_find_photos_done

<div class="yarn-node" data-title="task_find_photos_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś wszystkie zdjęcia!</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Wróć i porozmawiaj z przewodnikiem.</span> <span class="yarn-meta">#line:go_back #task:go_back </span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-cards-done"></a>

## task_collect_cards_done

<div class="yarn-node" data-title="task_collect_cards_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś wszystkie zdjęcia!</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Wróć i porozmawiaj z przewodnikiem.</span> <span class="yarn-meta">#shadow:go_back #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## final_quiz

<div class="yarn-node" data-title="final_quiz">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Odra to długa rzeka. Pamiętasz, dokąd trafia cała ta woda?</span> <span class="yarn-meta">#line:quiz1_intro</span>
<span class="yarn-line">Morze Bałtyckie</span> <span class="yarn-meta">#line:quiz_a1</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    Tak! Zgadza się! Odra płynie na północ przez Polskę i wpada do Morza Bałtyckiego.</span> <span class="yarn-meta">#line:quiz1_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>
<span class="yarn-line">Morze Śródziemne</span> <span class="yarn-meta">#line:quiz_a2</span>
<span class="yarn-line">    Hmm... to zdecydowanie za daleko na południe!</span> <span class="yarn-meta">#line:quiz1_fail1</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">Morze Czarne</span> <span class="yarn-meta">#line:quiz_a3</span>
<span class="yarn-line">    Nie do końca. Odra płynie na północ, a nie na południe!</span> <span class="yarn-meta">#line:quiz1_fail2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#line:dontknow</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span> 
<span class="yarn-line">    Spokojnie! Spójrz na niebieską linię na mapie.</span> <span class="yarn-meta">#line:quiz1_hint</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-2"></a>

## final_quiz_2

<div class="yarn-node" data-title="final_quiz_2">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Czy Odra jest *najdłuższą* rzeką w Polsce?</span> <span class="yarn-meta">#line:quiz2_intro</span>
<span class="yarn-line">Nie, Wisła jest dłuższa</span> <span class="yarn-meta">#line:quiz2_a1</span>
    <span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">    Idealnie! Wisła jest numerem jeden, Odra jest drugą najdłuższą rzeką.</span> <span class="yarn-meta">#line:quiz2_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-line">Tak, to jest najdłuższy</span> <span class="yarn-meta">#line:quiz2_a2</span>
<span class="yarn-line">    Jest bardzo duża, ale jest jedna rzeka, która jest jeszcze dłuższa.</span> <span class="yarn-meta">#line:quiz2_fail</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-river-sign"></a>

## npc_river_sign

<div class="yarn-node" data-title="npc_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- RIVER SIGN</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $river_sign &lt; 10&gt;&gt;
<span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">Spójrz na duży niebieski znak przy moście.</span> <span class="yarn-meta">#line:sign_1</span>
<span class="yarn-line">Co mówią nam białe faliste linie?</span> <span class="yarn-meta">#line:sign_3</span>
<span class="yarn-line">Tutaj płynie rzeka</span> <span class="yarn-meta">#line:sign_4</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_river_sign&gt;&gt;</span>
<span class="yarn-line">    Tak! Te niebieskie fale są uniwersalnym znakiem rzeki.</span> <span class="yarn-meta">#line:sign_5</span>
<span class="yarn-line">Tam most się porusza</span> <span class="yarn-meta">#line:sign_6</span>
<span class="yarn-line">    Nie. Spróbuj ponownie.</span> <span class="yarn-meta">#line:try_again </span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#line:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-river-sign"></a>

## chest_river_sign

<div class="yarn-node" data-title="chest_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $river_sign == 1&gt;&gt;</span>
<span class="yarn-line">    Antura pokryła znak błotem! Wytrzyj go do czysta, żeby zobaczyć fale.</span> <span class="yarn-meta">#line:ch_sign1</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity clean_river_sign chest_river_sign&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_sign&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">    Świetna robota! Teraz zawsze będziesz wiedział, kiedy przeprawiasz się przez rzekę w Europie.</span> <span class="yarn-meta">#line:ch_sign2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-odra-map"></a>

## npc_odra_map

<div class="yarn-node" data-title="npc_odra_map">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- ODRA RIVER MAP</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">Odra jest drugą pod względem długości rzeką w Polsce.</span> <span class="yarn-meta">#line:map_1</span>
&lt;&lt;if $map_odra &lt; 10&gt;&gt;
<span class="yarn-line">Czy Odra wpływa do gór czy do morza?</span> <span class="yarn-meta">#line:map_2</span>
<span class="yarn-line">Morze Bałtyckie</span> <span class="yarn-meta">#line:map_3</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_odra_map&gt;&gt;</span>
<span class="yarn-line">    Zgadza się! Płynie aż na północ.</span> <span class="yarn-meta">#line:map_4</span>
<span class="yarn-line">Morze Śródziemne</span> <span class="yarn-meta">#line:map_5</span>
<span class="yarn-line">    Nie. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_odra_map&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-odra-map"></a>

## chest_odra_map

<div class="yarn-node" data-title="chest_odra_map">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $map_odra == 1&gt;&gt;</span>
<span class="yarn-line">    Udowodnij, że wiesz, dokąd płynie rzeka!</span> <span class="yarn-meta">#line:ch_map1</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_odra_facts chest_odra_river_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $map_odra == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_map&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    Mapa wróciła! Pokazuje ujście Odry do Morza Bałtyckiego.</span> <span class="yarn-meta">#line:ch_map2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-tumski-bridge"></a>

## npc_tumski_bridge

<div class="yarn-node" data-title="npc_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TUMSKI BRIDGE (The Romantic Bridge)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">To jest Most Tumski. Prowadzi do najstarszej części miasta.</span> <span class="yarn-meta">#line:tum_1</span>
<span class="yarn-line">Każdego wieczoru mężczyzna ręcznie zapala 102 latarnie gazowe!</span> <span class="yarn-meta">#line:tum_2</span>
&lt;&lt;if $bridge_tumski &lt; 10&gt;&gt;
<span class="yarn-line">Co pary wieszają na tym moście, aby zapewnić sobie szczęście i miłość?</span> <span class="yarn-meta">#line:tum_3</span>
<span class="yarn-line">Kłódki</span> <span class="yarn-meta">#line:tum_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Tak! Choć są bardzo ciężkie i zostaną usunięte!</span> <span class="yarn-meta">#line:tum_5</span>
<span class="yarn-line">Mokre skarpetki</span> <span class="yarn-meta">#line:tum_6</span>
<span class="yarn-line">    To nie byłoby zbyt romantyczne! Spróbuj jeszcze raz.</span> <span class="yarn-meta">#line:fail_tum</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-tumski-bridge"></a>

## chest_tumski_bridge

<div class="yarn-node" data-title="chest_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_tumski == 1&gt;&gt;</span>
<span class="yarn-line">    Wyczyśćcie rdzę z tego starego żelaznego mostu!</span> <span class="yarn-meta">#line:ch_tum1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tumski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-redzinski-bridge"></a>

## npc_redzinski_bridge

<div class="yarn-node" data-title="npc_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- RĘDZIŃSKI BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Most Rędziński jest najwyższym i najdłuższym mostem w Polsce.</span> <span class="yarn-meta">#line:redz_1</span>
<span class="yarn-line">Ma 122 metry wysokości. Jest wyższy niż Katedra!</span> <span class="yarn-meta">#line:redz_2</span>
&lt;&lt;if $bridge_redzinski &lt; 10&gt;&gt;
<span class="yarn-line">Co podtrzymuje ten potężny most?</span> <span class="yarn-meta">#line:redz_3</span>
<span class="yarn-line">Liny stalowe</span> <span class="yarn-meta">#line:redz_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Zgadza się! Ten gigantyczny most podtrzymują mocne liny. Samochody jeżdżą nim po mieście.</span> <span class="yarn-meta">#line:redz_5</span>
<span class="yarn-line">Magnesy i magia</span> <span class="yarn-meta">#line:redz_6</span>
<span class="yarn-line">    Wygląda to jak magia, ale tak naprawdę to inżynieria!</span> <span class="yarn-meta">#line:fail_redz</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-redzinski-bridge"></a>

## chest_redzinski_bridge

<div class="yarn-node" data-title="chest_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_redzinski == 1&gt;&gt;</span>
<span class="yarn-line">    Odbudujmy najwyższy pylon we Wrocławiu!</span> <span class="yarn-meta">#line:ch_redz1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_redzinski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-train-bridge"></a>

## npc_train_bridge

<div class="yarn-node" data-title="npc_train_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TRAIN BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">Pociągi przemierzają Odrę we Wrocławiu od ponad 150 lat.</span> <span class="yarn-meta">#line:train_1</span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">Dlaczego mosty kolejowe są wykonane z tak ciężkiej stali?</span> <span class="yarn-meta">#line:train_2</span>
<span class="yarn-line">Ponieważ pociągi są bardzo ciężkie</span> <span class="yarn-meta">#line:train_3</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Tak! Musi być wystarczająco wytrzymały, żeby wytrzymać ciężkie pociągi.</span> <span class="yarn-meta">#line:train_4</span>
<span class="yarn-line">Wydawać głośny dźwięk</span> <span class="yarn-meta">#line:train_5</span>
<span class="yarn-line">    Są głośne, ale nie dlatego!</span> <span class="yarn-meta">#line:fail_train</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_train_bridge&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-train"></a>

## chest_bridge_train

<div class="yarn-node" data-title="chest_bridge_train">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_train == 1&gt;&gt;</span>
<span class="yarn-line">    Dopasuj ciężkie pudła do torów kolejowych!</span> <span class="yarn-meta">#line:ch_train1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-houseboat"></a>

## npc_houseboat

<div class="yarn-node" data-title="npc_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- HOUSEBOAT</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">We Wrocławiu niektórzy nazywają rzekę swoją „ulicą domową”.</span> <span class="yarn-meta">#line:house_1</span>
<span class="yarn-line">Nawet krasnoludy pokochałyby latający dom!</span> <span class="yarn-meta">#line:house_2</span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">Jeśli mieszkasz na łodzi mieszkalnej, co wykorzystujesz na podwórku?</span> <span class="yarn-meta">#line:house_3</span>
<span class="yarn-line">Rzeka Odra</span> <span class="yarn-meta">#line:house_4</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_houseboat&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#shadow:chest_opens </span>
<span class="yarn-line">Las na dachu</span> <span class="yarn-meta">#line:house_6</span>
<span class="yarn-line">    Nie. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_houseboat&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-houseboat"></a>

## chest_houseboat

<div class="yarn-node" data-title="chest_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_house == 1&gt;&gt;</span>
<span class="yarn-line">    Napraw okna w pływającym domu!</span> <span class="yarn-meta">#line:ch_house1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_houseboat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_houseboat&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">    Przytulny dom nad Odrą! Zdjęcie zebrane.</span> <span class="yarn-meta">#line:ch_house2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-people"></a>

## npc_boat_people

<div class="yarn-node" data-title="npc_boat_people">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BOAT PEOPLE (Tourist Boats)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">Łodzie turystyczne zabierają ludzi na wycieczkę do zoo i katedry.</span> <span class="yarn-meta">#line:tour_1</span>
&lt;&lt;if $boat_tourist &lt; 10&gt;&gt;
<span class="yarn-line">Czego używają ludzie na tych statkach, żeby podziwiać widoki?</span> <span class="yarn-meta">#line:tour_2</span>
<span class="yarn-line">Ich oczy i kamery</span> <span class="yarn-meta">#line:tour_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Tak! Uśmiechnij się do zdjęcia!</span> <span class="yarn-meta">#line:tour_4</span>
<span class="yarn-line">Peryskop</span> <span class="yarn-meta">#line:tour_5</span>
<span class="yarn-line">    Jeszcze nie jesteśmy pod wodą! Spróbuj ponownie.</span> <span class="yarn-meta">#line:fail_tour</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-people"></a>

## chest_boat_people

<div class="yarn-node" data-title="chest_boat_people">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_tourist == 1&gt;&gt;</span>
<span class="yarn-line">    Znajdź turystów ukrytych na pokładzie!</span> <span class="yarn-meta">#line:ch_tour1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tourist&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-barge"></a>

## npc_boat_barge

<div class="yarn-node" data-title="npc_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BARGE (Cargo)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Od stuleci barki (Barki) służyły do ​​transportu węgla i piasku Odrą.</span> <span class="yarn-meta">#line:barge_1</span>
&lt;&lt;if $boat_barge &lt; 10&gt;&gt;
<span class="yarn-line">Barka jest bardzo płaska. Dlaczego?</span> <span class="yarn-meta">#line:barge_2</span>
<span class="yarn-line">Noszenie ciężkich przedmiotów, nawet gdy woda nie jest głęboka.</span> <span class="yarn-meta">#line:barge_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_barge&gt;&gt;</span>
<span class="yarn-line">    Dokładnie! To ciężarówka, która pływa.</span> <span class="yarn-meta">#line:barge_4</span>
<span class="yarn-line">Więc może się ukryć przed krasnoludami</span> <span class="yarn-meta">#line:barge_5</span>
<span class="yarn-line">    Nie. Spróbuj ponownie.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_barge&gt;&gt;</span>
<span class="yarn-line">Nie wiem</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-barge"></a>

## chest_boat_barge

<div class="yarn-node" data-title="chest_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_barge == 1&gt;&gt;</span>
<span class="yarn-line">    Zagraj w minigrę, aby otworzyć skrzynię!</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_barge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_barge&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">    Skrzynia się otwiera. Znajdziesz zdjęcie!</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest pusta.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Skrzynia jest zamknięta.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kayak"></a>

## spawned_kayak

<div class="yarn-node" data-title="spawned_kayak">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chcę popływać małym KAJAKIEM.</span> <span class="yarn-meta">#line:0f36b7f #card:kayak</span>
<span class="yarn-line">Kajaki są świetne do odkrywania natury!</span> <span class="yarn-meta">#line:kayak_2 #card:kayak</span>
<span class="yarn-line">Wiosłowanie to świetna zabawa i dobre ćwiczenie!</span> <span class="yarn-meta">#line:kayak_3</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tak wiele MOSTÓW w tym mieście.</span> <span class="yarn-meta">#line:0577d80 </span>
<span class="yarn-line">Wrocław jest naprawdę piękny.</span> <span class="yarn-meta">#line:089ea37 </span>
<span class="yarn-line">Uwielbiam pierogi!</span> <span class="yarn-meta">#line:07ff8c5 </span>
<span class="yarn-line">Wyspa Katedralna nocą wygląda magicznie.</span> <span class="yarn-meta">#line:tourist_4</span>

</code>
</pre>
</div>


