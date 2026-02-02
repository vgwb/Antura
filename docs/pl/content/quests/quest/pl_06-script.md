---
title: Pierniki i targ spożywczy (pl_06) - Script
hide:
---

# Pierniki i targ spożywczy (pl_06) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-06-gingerbread-food-market/37/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1211829352#gid=1211829352)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_06%20Torun%20Market/PL_06%20Torun%20Market%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_06 | Market (Torun)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - torun_gingerbread (cultural tradition)</span>
<span class="yarn-header-dim">// - torun_town_hall (Gothic architecture)</span>
<span class="yarn-header-dim">// - medieval_market (historical setting)</span>
<span class="yarn-header-dim">// - pierogi</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - bake gingerbread (order/memory of ingredients)</span>
<span class="yarn-header-dim">// - Pierogi Challenge: order/memory of ingredients (flour, eggs, cheese, potatoes)</span>
<span class="yarn-header-dim">// Words used: Toruń, market, vendor, grocer, beekeeper, dairy, eggs, milk, butter, flour, honey, cloves, cinnamon, ginger, pierogi, molds, coins, zloty, kitchen, gingerbread, medieval</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $grocer_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $beekeeper_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $cheesemonger_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $eggvendor_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spicevendor_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $gingerbread_done = false&gt;&gt;</span>

<span class="yarn-line">Jesteśmy w TORUNIU, mieście legendarnych polskich pierników!</span> <span class="yarn-meta">#line:080555e </span>
<span class="yarn-line">Przyjrzyjmy się RYNKOWI.</span> <span class="yarn-meta">#line:03e11fa </span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>

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
<span class="yarn-line">To zadanie zostało ukończone.</span> <span class="yarn-meta">#line:073978d</span>
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
<span class="yarn-line">Chcesz spróbować upiec PIERNIKI?</span> <span class="yarn-meta">#line:09722f2 </span>
<span class="yarn-line">A może przepis na PIEROGI?</span> <span class="yarn-meta">#line:0954c65 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook"></a>

## cook

<div class="yarn-node" data-title="cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetCollectedItem("COLLECT_INGREDIENTS") &gt;= 7&gt;&gt;
   <span class="yarn-cmd">&lt;&lt;jump cook_ingredients_done&gt;&gt;</span>
&lt;&lt;elseif GetCollectedItem("COLLECT_INGREDIENTS") &gt; 0&gt;&gt;
   <span class="yarn-cmd">&lt;&lt;jump cook_not_enough&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;jump cook_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-welcome"></a>

## cook_welcome

<div class="yarn-node" data-title="cook_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card cook&gt;&gt;</span>
<span class="yarn-line">Dzień dobry! Jestem kucharzem i chcę upiec pierniki.</span> <span class="yarn-meta">#line:028131b</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Mogę upiec dla Ciebie nasze słynne PIERNIKI.</span> <span class="yarn-meta">#line:07fb019</span>
<span class="yarn-line">Ale potrzebuję SKŁADNIKÓW.</span> <span class="yarn-meta">#line:00a46f0</span>
<span class="yarn-cmd">&lt;&lt;jump task_ingredients&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients"></a>

## task_ingredients

<div class="yarn-node" data-title="task_ingredients">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  currency_zloty&gt;&gt;</span>
<span class="yarn-line">Proszę wziąć te pieniądze</span> <span class="yarn-meta">#line:0ff272d </span>
<span class="yarn-cmd">&lt;&lt;card market_traders&gt;&gt;</span>
<span class="yarn-line">Idź na TARG.</span> <span class="yarn-meta">#line:0c35a9c</span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">Rozmawiając z ludźmi pamiętaj o dobrych manierach!</span> <span class="yarn-meta">#line:03f1020 </span>
<span class="yarn-line">Witając kogoś, mówimy „Dzień dobry”.</span> <span class="yarn-meta">#line:091bb57 </span>
<span class="yarn-line">i "Dziękuję", aby komuś podziękować.</span> <span class="yarn-meta">#line:076ffac </span>
<span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-desc"></a>

## task_ingredients_desc

<div class="yarn-node" data-title="task_ingredients_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Kup składniki potrzebne do przepisu.</span> <span class="yarn-meta">#line:00565e5 </span>
<span class="yarn-line">MĄKA</span> <span class="yarn-meta">#line:070b733 </span>
<span class="yarn-line">MIÓD</span> <span class="yarn-meta">#line:0e22cab </span>
<span class="yarn-line">CUKIER</span> <span class="yarn-meta">#line:055af31</span>
<span class="yarn-line">MASŁO</span> <span class="yarn-meta">#line:0bfb896 </span>
<span class="yarn-line">Jedno jajko</span> <span class="yarn-meta">#line:0de2001 </span>
<span class="yarn-line">CYNAMON</span> <span class="yarn-meta">#line:0ba5cca </span>
<span class="yarn-line">I na koniec trochę IMBIRU</span> <span class="yarn-meta">#line:0a700e3 </span>
<span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-not-enough"></a>

## cook_not_enough

<div class="yarn-node" data-title="cook_not_enough">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nie masz wszystkich SKŁADNIKÓW!</span> <span class="yarn-meta">#line:003f24d </span>
<span class="yarn-line">Odwiedź wszystkich SPRZEDAWCÓW na RYNKU.</span> <span class="yarn-meta">#line:0d691b9 </span>
<span class="yarn-line">I weź składniki, które kupiłeś.</span> <span class="yarn-meta">#line:0ea58f3 </span>

</code>
</pre>
</div>

<a id="ys-node-task-ingredients-done"></a>

## task_ingredients_done

<div class="yarn-node" data-title="task_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie. Masz WSZYSTKIE SKŁADNIKI!</span> <span class="yarn-meta">#line:03f7a4a</span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>
<span class="yarn-line">Wróć do KUCHARZA.</span> <span class="yarn-meta">#line:0222915 </span>
<span class="yarn-cmd">&lt;&lt;task_start go_back_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cook-ingredients-done"></a>

## cook_ingredients_done

<div class="yarn-node" data-title="cook_ingredients_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobrze, masz wszystko.</span> <span class="yarn-meta">#line:0bb1792</span>
<span class="yarn-line">I byłeś bardzo uprzejmy.</span> <span class="yarn-meta">#line:0c400be </span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Teraz możemy upiec TORUŃSKIE PIERNIKI!</span> <span class="yarn-meta">#line:0b5d503</span>
<span class="yarn-cmd">&lt;&lt;jump activity_bake_gingerbread&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-bake-gingerbread"></a>

## activity_bake_gingerbread

<div class="yarn-node" data-title="activity_bake_gingerbread">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread_mold&gt;&gt;</span>
<span class="yarn-line">Dopasuj części piernika do sprzedawcy.</span> <span class="yarn-meta">#line:0e54683 </span>
<span class="yarn-cmd">&lt;&lt;activity match_ingredients_pl_06 activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_ingredients_pl_06") &gt; 0&gt;&gt;
<span class="yarn-line">Brawo! Udało Ci się dopasować wszystkie składniki.</span> <span class="yarn-meta">#line:01648b2</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">A teraz czas na pieczenie PIERNIKÓW!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-line">Zapraszmy do wejścia do Starego ratusza. Czeka tam na Was niespodzianka.</span> <span class="yarn-meta">#line:01c48d3</span>
<span class="yarn-cmd">&lt;&lt;set $gingerbread_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_castle&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Nie jest idealnie. Spróbuj ponownie!</span> <span class="yarn-meta">#line:007427a </span>
<span class="yarn-cmd">&lt;&lt;jump activity_bake_gingerbread&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer"></a>

## grocer

<div class="yarn-node" data-title="grocer">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// GROCER</span>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_grocer_pl_06") &gt; 0&gt;&gt;
<span class="yarn-line">   Już u mnie kupiłeś!</span> <span class="yarn-meta">#line:already_bought</span>
<span class="yarn-line">   Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">   Tak</span> <span class="yarn-meta">#line:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NIE</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#line:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#line:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump grocer_bonjour&gt;&gt;</span>
<span class="yarn-line">Dobranoc!</span> <span class="yarn-meta">#line:0b4db3b </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-bonjour"></a>

## grocer_bonjour

<div class="yarn-node" data-title="grocer_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_grocer&gt;&gt;</span>
<span class="yarn-line">Cześć! Jestem sprzedawcą spożywczym. Sprzedaję wiele rodzajów żywności.</span> <span class="yarn-meta">#line:0ffbfa4</span>
<span class="yarn-line">Czego potrzebujesz?</span> <span class="yarn-meta">#line:0c6a554 </span>
<span class="yarn-cmd">&lt;&lt;jump grocer_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-question"></a>

## grocer_question

<div class="yarn-node" data-title="grocer_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#line:what_to_buy </span>
<span class="yarn-line">Ryba</span> <span class="yarn-meta">#line:0d6dabd </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mięso</span> <span class="yarn-meta">#line:03eeda4 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Sukienka</span> <span class="yarn-meta">#line:097fca2 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mąka i cukier</span> <span class="yarn-meta">#line:0068f15 </span>
   <span class="yarn-cmd">&lt;&lt;jump grocer_pay_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-pay-activity"></a>

## grocer_pay_activity

<div class="yarn-node" data-title="grocer_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz odpowiednie monety do zapłaty.</span> <span class="yarn-meta">#line:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer_pl_06 grocer_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-grocer-payment-done"></a>

## grocer_payment_done

<div class="yarn-node" data-title="grocer_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: grocer</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Położyłem twoje rzeczy na stole. Dzięki!</span> <span class="yarn-meta">#line:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:goodbye</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Miłego dnia!</span> <span class="yarn-meta">#line:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Grocer&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper"></a>

## beekeper

<div class="yarn-node" data-title="beekeper">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// BEEKEPER</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_beekeper") &gt; 0&gt;&gt;
<span class="yarn-line">   Już u mnie kupiłeś!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   Tak</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump beekeper_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#line:06b0535 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-bonjour"></a>

## beekeper_bonjour

<div class="yarn-node" data-title="beekeper_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_beekeper&gt;&gt;</span>
<span class="yarn-line">Dzień dobry! Jestem pszczelarzem i sprzedaję miód.</span> <span class="yarn-meta">#line:04b4a87</span>
<span class="yarn-cmd">&lt;&lt;card honey&gt;&gt;</span>
<span class="yarn-line">Wszystkie moje produkty pochodzą z moich uli!</span> <span class="yarn-meta">#line:0aa9ce7</span>
<span class="yarn-cmd">&lt;&lt;jump beekeper_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-question"></a>

## beekeper_question

<div class="yarn-node" data-title="beekeper_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#shadow:what_to_buy </span>
<span class="yarn-line">Miód</span> <span class="yarn-meta">#line:honey</span>
   <span class="yarn-cmd">&lt;&lt;jump beekeper_pay_activity&gt;&gt;</span>
<span class="yarn-line">Czekolada</span> <span class="yarn-meta">#line:chocolate</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:bread</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Mleko</span> <span class="yarn-meta">#line:milk</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-pay-activity"></a>

## beekeper_pay_activity

<div class="yarn-node" data-title="beekeper_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz odpowiednie monety do zapłaty.</span> <span class="yarn-meta">#shadow:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_beekeper beekeper_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-beekeper-payment-done"></a>

## beekeper_payment_done

<div class="yarn-node" data-title="beekeper_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: beekeper</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Położyłem twoje rzeczy na stole. Dzięki!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Miłego dnia!</span> <span class="yarn-meta">#shadow:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Beekeper&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger"></a>

## cheesemonger

<div class="yarn-node" data-title="cheesemonger">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ---------------------------------------------- </span>
<span class="yarn-header-dim">// CHEESEMONGER</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_cheesemonger_pl_06") &gt; 0&gt;&gt;
<span class="yarn-line">   Już u mnie kupiłeś!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   Tak</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-bonjour"></a>

## cheesemonger_bonjour

<div class="yarn-node" data-title="cheesemonger_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć! Sprzedaję ser i masło.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">Używam zarówno mleka krowiego, jak i koziego.</span> <span class="yarn-meta">#line:02f4bc9 </span>
<span class="yarn-cmd">&lt;&lt;card person_cheesemonger&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump cheesemonger_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-question"></a>

## cheesemonger_question

<div class="yarn-node" data-title="cheesemonger_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#shadow:what_to_buy </span>
<span class="yarn-line">Masło</span> <span class="yarn-meta">#line:butter </span>
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Olej</span> <span class="yarn-meta">#line:057f694 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:087919f </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomidory</span> <span class="yarn-meta">#line:067bfab </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-pay-activity"></a>

## cheesemonger_pay_activity

<div class="yarn-node" data-title="cheesemonger_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz odpowiednie monety do zapłaty.</span> <span class="yarn-meta">#shadow:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger_pl_06 cheesemonger_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-cheesemonger-payment-done"></a>

## cheesemonger_payment_done

<div class="yarn-node" data-title="cheesemonger_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cheesemonger</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Położyłem twoje rzeczy na stole. Dzięki!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Miłego dnia!</span> <span class="yarn-meta">#shadow:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Cheesemonger&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor"></a>

## eggvendor

<div class="yarn-node" data-title="eggvendor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// EGG VENDOR</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_eggvendor") &gt; 0&gt;&gt;
<span class="yarn-line">   Już u mnie kupiłeś!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   Tak</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-bonjour"></a>

## eggvendor_bonjour

<div class="yarn-node" data-title="eggvendor_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_eggvendor&gt;&gt;</span>
<span class="yarn-line">Cześć! Sprzedaję JAJKA. Jestem sprzedawcą jajek.</span> <span class="yarn-meta">#line:09a9960 </span>
<span class="yarn-cmd">&lt;&lt;jump eggvendor_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-question"></a>

## eggvendor_question

<div class="yarn-node" data-title="eggvendor_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#shadow:what_to_buy</span>
<span class="yarn-line">Jajka</span> <span class="yarn-meta">#line:eggs</span>
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Olej</span> <span class="yarn-meta">#line:06cc62e </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Chleb</span> <span class="yarn-meta">#line:059920e </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-pay-activity"></a>

## eggvendor_pay_activity

<div class="yarn-node" data-title="eggvendor_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz odpowiednie monety do zapłaty.</span> <span class="yarn-meta">#shadow:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_eggvendor eggvendor_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eggvendor-payment-done"></a>

## eggvendor_payment_done

<div class="yarn-node" data-title="eggvendor_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: eggvendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Położyłem twoje rzeczy na stole. Dzięki!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Miłego dnia!</span> <span class="yarn-meta">#shadow:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Eggvendor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor"></a>

## spicevendor

<div class="yarn-node" data-title="spicevendor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// SPICE VENDOR</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("money_spicevendor") &gt; 0&gt;&gt;
<span class="yarn-line">   Już u mnie kupiłeś!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Czy chcesz zagrać jeszcze raz?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   Tak</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NIE</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Dzień dobry!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_bonjour&gt;&gt;</span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-bonjour"></a>

## spicevendor_bonjour

<div class="yarn-node" data-title="spicevendor_bonjour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_spicevendor&gt;&gt;</span>
<span class="yarn-line">Cześć! Sprzedaję przyprawy.</span> <span class="yarn-meta">#line:0f83873 </span>
<span class="yarn-cmd">&lt;&lt;jump spicevendor_question&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-question"></a>

## spicevendor_question

<div class="yarn-node" data-title="spicevendor_question">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Co chcesz kupić?</span> <span class="yarn-meta">#shadow:what_to_buy</span>
<span class="yarn-line">Cynamon i imbir</span> <span class="yarn-meta">#line:0fe40e7 </span>
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Masło</span> <span class="yarn-meta">#line:0fa399a </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Miód</span> <span class="yarn-meta">#line:0cec0d0 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-pay-activity"></a>

## spicevendor_pay_activity

<div class="yarn-node" data-title="spicevendor_pay_activity">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz odpowiednie monety do zapłaty.</span> <span class="yarn-meta">#shadow:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_spicevendor spicevendor_payment_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spicevendor-payment-done"></a>

## spicevendor_payment_done

<div class="yarn-node" data-title="spicevendor_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spicevendor</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Położyłem twoje rzeczy na stole. Dzięki!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Do widzenia!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Dziękuję!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Miłego dnia!</span> <span class="yarn-meta">#shadow:nice_day</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Spicevendor&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-understand"></a>

## talk_dont_understand

<div class="yarn-node" data-title="talk_dont_understand">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// GENERIC DETOURS</span>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przepraszam, nie rozumiem...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">Co?</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Co?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-dont-sell"></a>

## talk_dont_sell

<div class="yarn-node" data-title="talk_dont_sell">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Przykro mi, ale tego nie sprzedaję.</span> <span class="yarn-meta">#line:08700b0 </span>

</code>
</pre>
</div>

<a id="ys-node-hard-payment-done"></a>

## hard_payment_done

<div class="yarn-node" data-title="hard_payment_done">
<pre class="yarn-code" style="--node-color:orange"><code>
<span class="yarn-header-dim">tags: detour</span>
<span class="yarn-header-dim">color: orange</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Spróbuj porozmawiać również z innymi sprzedawcami.</span> <span class="yarn-meta">#line:06ae965 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flour"></a>

## item_flour

<div class="yarn-node" data-title="item_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// ITEMS TO COLLECT</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_bread&gt;&gt;</span>
<span class="yarn-line">Mąka</span> <span class="yarn-meta">#line:08e101e </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-honey"></a>

## item_honey

<div class="yarn-node" data-title="item_honey">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_honey&gt;&gt;</span>
<span class="yarn-line">Miód</span> <span class="yarn-meta">#line:0817d3c </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-sugar"></a>

## item_sugar

<div class="yarn-node" data-title="item_sugar">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_sugar&gt;&gt;</span>
<span class="yarn-line">Cukier</span> <span class="yarn-meta">#line:05ad31e </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-butter"></a>

## item_butter

<div class="yarn-node" data-title="item_butter">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_butter&gt;&gt;</span>
<span class="yarn-line">Masło</span> <span class="yarn-meta">#line:0a8a8cc </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-egg"></a>

## item_egg

<div class="yarn-node" data-title="item_egg">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_egg&gt;&gt;</span>
<span class="yarn-line">Jajko</span> <span class="yarn-meta">#line:00ab8e2 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-cinnamon"></a>

## item_cinnamon

<div class="yarn-node" data-title="item_cinnamon">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_cinnamon&gt;&gt;</span>
<span class="yarn-line">Cynamon</span> <span class="yarn-meta">#line:0f00ddd </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-item-ginger"></a>

## item_ginger

<div class="yarn-node" data-title="item_ginger">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_ginger&gt;&gt;</span>
<span class="yarn-line">Imbir</span> <span class="yarn-meta">#line:08049d5 </span>
<span class="yarn-cmd">&lt;&lt;collect&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-door-castle"></a>

## door_castle

<div class="yarn-node" data-title="door_castle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// ----------------------------------------------</span>
<span class="yarn-header-dim">// PIEROGI</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Drzwi są zamknięte.</span> <span class="yarn-meta">#line:042ecfc </span>

</code>
</pre>
</div>

<a id="ys-node-npc-castle"></a>

## npc_castle

<div class="yarn-node" data-title="npc_castle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Cześć. W czym mogę pomóc?</span> <span class="yarn-meta">#line:0f11caf </span>
<span class="yarn-line">Chcę wejść do Starego Ratusza.</span> <span class="yarn-meta">#line:0449db3 </span>
   <span class="yarn-cmd">&lt;&lt;if $gingerbread_done == true&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;trigger open_door_castle&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;area area_castle&gt;&gt;</span>
<span class="yarn-line">      Proszę, wejdź! Ktoś na ciebie czeka.</span> <span class="yarn-meta">#line:0716b5d </span>
      <span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;SetInteractable door_castle false&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">      Stary Ratusz jest zamknięty dla zwiedzających.</span> <span class="yarn-meta">#line:096470a </span>
   <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Po prostu się rozglądam.</span> <span class="yarn-meta">#line:09a9858 </span>
<span class="yarn-line">   Dobrze, miłego dnia.</span> <span class="yarn-meta">#line:09ee9bf </span>

</code>
</pre>
</div>

<a id="ys-node-npc-pierogi"></a>

## npc_pierogi

<div class="yarn-node" data-title="npc_pierogi">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: cook</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">tags: noRepeatLastLine</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("clean_castle")&gt;&gt;</span>
<span class="yarn-line">    Dziękujemy za sprzątanie! Teraz możemy rozpocząć Festiwal Pierogów!</span> <span class="yarn-meta">#line:07e852b </span>
    <span class="yarn-cmd">&lt;&lt;card pierogi&gt;&gt;</span>
<span class="yarn-line">    Czy kiedykolwiek próbowałeś PIEROGÓW?</span> <span class="yarn-meta">#line:0da391b </span>
<span class="yarn-line">    Tak</span> <span class="yarn-meta">#line:08ac4ff </span>
<span class="yarn-line">      Są pyszne, prawda?</span> <span class="yarn-meta">#line:040912f </span>
<span class="yarn-line">    NIE</span> <span class="yarn-meta">#line:0cb270e </span>
<span class="yarn-line">      To specjalny rodzaj pierożka wypełnionego pysznościami, np. serem lub ziemniakami.</span> <span class="yarn-meta">#line:09da259 </span>
    <span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_tower&gt;&gt;</span>
<span class="yarn-line">    Widziałem, jak ten wielki niebieski pies wchodził po schodach!</span> <span class="yarn-meta">#line:0ee5d9b </span>
<span class="yarn-line">    Znajdź Anturę!</span> <span class="yarn-meta">#line:09877a9 </span>
<span class="yarn-line">    Włączyłem dla ciebie windę.</span> <span class="yarn-meta">#line:0f5e348 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;SetInteractable tower_lever true&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;task_start climb_the_tower antura_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Cześć. Chcemy otworzyć Festiwal Pierogów</span> <span class="yarn-meta">#line:09c418a </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_trash&gt;&gt;</span>
<span class="yarn-line">    Ale ten wielki niebieski pies narobił bałaganu w holu.</span> <span class="yarn-meta">#line:067cefa </span>
<span class="yarn-line">    Czy możesz nam pomóc posprzątać salę?</span> <span class="yarn-meta">#line:003385d  </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_full&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura_hall false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start clean_castle task_clean_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-clean-done"></a>

## task_clean_done

<div class="yarn-node" data-title="task_clean_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dobra robota! Wróć do szefa kuchni.</span> <span class="yarn-meta">#line:08e9b5f </span>
<span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-antura-tower"></a>

## antura_tower

<div class="yarn-node" data-title="antura_tower">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Znalazłeś Anturę!</span> <span class="yarn-meta">#line:0307b40 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid"></a>

## spawned_kid

<div class="yarn-node" data-title="spawned_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Lubię słodkie PIERNIKI</span> <span class="yarn-meta">#line:087d4b0 </span>
<span class="yarn-line">Lubię STARY RYNEK!</span> <span class="yarn-meta">#line:0090fb8 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Stara Hala wygląda wspaniale.</span> <span class="yarn-meta">#line:041418b </span>
<span class="yarn-line">Dzisiaj spróbuję PIEROGÓW.</span> <span class="yarn-meta">#line:068a9a7</span>
<span class="yarn-line">Toruń jest naprawdę piękny!</span> <span class="yarn-meta">#line:08bc7dd </span>
<span class="yarn-line">Na rynku jest tak wesoło!</span> <span class="yarn-meta">#line:0d92388 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-buyer"></a>

## spawned_buyer

<div class="yarn-node" data-title="spawned_buyer">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Muszę kupić świeże składniki.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">Na rynku można kupić najlepsze produkty.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Świeża ryba jest najlepsza!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">Nie mogę się doczekać, aż przygotuję pyszny posiłek!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-currency"></a>

## spawned_currency

<div class="yarn-node" data-title="spawned_currency">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jeden złoty to 100 groszy.</span> <span class="yarn-meta">#line:0e6c526 #card:currency_zloty</span>
<span class="yarn-line">Złote mają różne rozmiary dla każdej wartości.</span> <span class="yarn-meta">#line:021819a #card:currency_zloty</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-jobs"></a>

## spawned_jobs

<div class="yarn-node" data-title="spawned_jobs">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: buyers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Piekarz piecze chleb.</span> <span class="yarn-meta">#line:0606dc5 #card:person_baker</span>
<span class="yarn-line">Sprzedawca przypraw sprzedaje przyprawy i zioła.</span> <span class="yarn-meta">#line:0ca5a9b #card:person_spicevendor</span>
<span class="yarn-line">Sprzedawca sera sprzedaje ser i mleko.</span> <span class="yarn-meta">#line:0b5b1c5 #card:person_cheesemonger</span>
<span class="yarn-line">Sprzedawca warzyw i owoców sprzedaje warzywa.</span> <span class="yarn-meta">#line:03b9a2e #card:person_greengrocer</span>

</code>
</pre>
</div>


