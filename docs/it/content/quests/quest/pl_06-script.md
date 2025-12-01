---
title: Pan di zenzero e mercato alimentare (pl_06) - Script
hide:
---

# Pan di zenzero e mercato alimentare (pl_06) - Script
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

<span class="yarn-line">Ci troviamo a TORUŃ, patria del leggendario PAN DI ZENZERO polacco!</span> <span class="yarn-meta">#line:080555e </span>
<span class="yarn-line">Esploriamo il MERCATO.</span> <span class="yarn-meta">#line:03e11fa </span>
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
<span class="yarn-line">Questa missione è completata.</span> <span class="yarn-meta">#line:073978d</span>
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
<span class="yarn-line">Vuoi provare a preparare un PAN DI ZENZERO?</span> <span class="yarn-meta">#line:09722f2 </span>
<span class="yarn-line">O la ricetta dei PIEROGI?</span> <span class="yarn-meta">#line:0954c65 </span>
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
<span class="yarn-line">Dzień dobry! Sono un CUOCO e voglio fare il PAN DI ZENZERO.</span> <span class="yarn-meta">#line:028131b</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Posso preparare per te il nostro famoso PAN DI ZENZERO.</span> <span class="yarn-meta">#line:07fb019</span>
<span class="yarn-line">Ma ho bisogno di alcuni INGREDIENTI.</span> <span class="yarn-meta">#line:00a46f0</span>
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
<span class="yarn-line">Per favore prendi questi soldi</span> <span class="yarn-meta">#line:0ff272d </span>
<span class="yarn-cmd">&lt;&lt;card market_traders&gt;&gt;</span>
<span class="yarn-line">E vai al MERCATO TRADERS</span> <span class="yarn-meta">#line:0c35a9c</span>
<span class="yarn-cmd">&lt;&lt;detour task_ingredients_desc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start COLLECT_INGREDIENTS task_ingredients_done&gt;&gt;</span>
<span class="yarn-line">Quando parli con le persone, ricorda le buone maniere!</span> <span class="yarn-meta">#line:03f1020 </span>
<span class="yarn-line">Diciamo "Dzień dobry" per salutare qualcuno</span> <span class="yarn-meta">#line:091bb57 </span>
<span class="yarn-line">e "Dziękuję" per ringraziarli.</span> <span class="yarn-meta">#line:076ffac </span>
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
<span class="yarn-line">Acquista gli ingredienti per la ricetta.</span> <span class="yarn-meta">#line:00565e5 </span>
<span class="yarn-line">Un po' di FARINA</span> <span class="yarn-meta">#line:070b733 </span>
<span class="yarn-line">Un po' di MIELE</span> <span class="yarn-meta">#line:0e22cab </span>
<span class="yarn-line">Un po' di ZUCCHERO</span> <span class="yarn-meta">#line:055af31</span>
<span class="yarn-line">Un po' di BURRO</span> <span class="yarn-meta">#line:0bfb896 </span>
<span class="yarn-line">Un UOVO</span> <span class="yarn-meta">#line:0de2001 </span>
<span class="yarn-line">Un po' di CANNELLA</span> <span class="yarn-meta">#line:0ba5cca </span>
<span class="yarn-line">E infine un po' di ZENZERO</span> <span class="yarn-meta">#line:0a700e3 </span>
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
<span class="yarn-line">Non hai tutti gli INGREDIENTI!</span> <span class="yarn-meta">#line:003f24d </span>
<span class="yarn-line">Visita tutti i VENDITORI del MERCATO.</span> <span class="yarn-meta">#line:0d691b9 </span>
<span class="yarn-line">E prendi gli ingredienti che compri.</span> <span class="yarn-meta">#line:0ea58f3 </span>

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
<span class="yarn-line">Ottimo. Hai tutti gli INGREDIENTI!</span> <span class="yarn-meta">#line:03f7a4a</span>
<span class="yarn-cmd">&lt;&lt;target npc_cook&gt;&gt;</span>
<span class="yarn-line">Torniamo al CUOCO.</span> <span class="yarn-meta">#line:0222915 </span>
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
<span class="yarn-line">Ben fatto, hai tutto.</span> <span class="yarn-meta">#line:0bb1792</span>
<span class="yarn-line">E sei stato molto gentile.</span> <span class="yarn-meta">#line:0c400be </span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Ora possiamo preparare il PAN DI ZENZERO DI TORUŃ!</span> <span class="yarn-meta">#line:0b5d503</span>
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
<span class="yarn-line">Abbina i pezzi di GINGERBREAD al loro venditore.</span> <span class="yarn-meta">#line:0e54683 </span>
<span class="yarn-cmd">&lt;&lt;activity match_ingredients activity_match_done&gt;&gt;</span>

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
&lt;&lt;if GetActivityResult("match_ingredients") &gt; 0&gt;&gt;
<span class="yarn-line">Ottimo lavoro! Hai abbinato tutti gli ingredienti.</span> <span class="yarn-meta">#line:01648b2</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Adesso prepariamo il PAN DI ZENZERO!</span> <span class="yarn-meta">#line:0f0f617 </span>
<span class="yarn-line">Entrate nel Vecchio Municipio. Vi aspetta una sorpresa!</span> <span class="yarn-meta">#line:01c48d3</span>
<span class="yarn-cmd">&lt;&lt;set $gingerbread_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_castle&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Non è perfetto. Riprova!</span> <span class="yarn-meta">#line:007427a </span>
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
&lt;&lt;if GetActivityResult("money_grocer") &gt; 0&gt;&gt;
<span class="yarn-line">   Hai già acquistato da me!</span> <span class="yarn-meta">#line:already_bought</span>
<span class="yarn-line">   Vuoi giocare di nuovo?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">   SÌ</span> <span class="yarn-meta">#line:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NO</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#line:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#line:hello </span>
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
<span class="yarn-line">Ciao! Sono un commerciante di generi alimentari. Vendo molti tipi di prodotti alimentari.</span> <span class="yarn-meta">#line:0ffbfa4</span>
<span class="yarn-line">Di che cosa hai bisogno?</span> <span class="yarn-meta">#line:0c6a554 </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#line:what_to_buy </span>
<span class="yarn-line">Pescare</span> <span class="yarn-meta">#line:0d6dabd </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Carne</span> <span class="yarn-meta">#line:03eeda4 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Vestito</span> <span class="yarn-meta">#line:097fca2 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Farina e zucchero</span> <span class="yarn-meta">#line:0068f15 </span>
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
<span class="yarn-line">Scegli le monete giuste per pagare.</span> <span class="yarn-meta">#line:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_grocer grocer_payment_done&gt;&gt;</span>

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
<span class="yarn-line">Ho messo i tuoi oggetti sul tavolo. Grazie!</span> <span class="yarn-meta">#line:0567082 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:goodbye</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Buona giornata!</span> <span class="yarn-meta">#line:nice_day</span>
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
<span class="yarn-line">   Hai già acquistato da me!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Vuoi giocare di nuovo?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   SÌ</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NO</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump beekeper_bonjour&gt;&gt;</span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#line:06b0535 </span>
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
<span class="yarn-line">Buongiorno! Sono un apicoltore e vendo miele.</span> <span class="yarn-meta">#line:04b4a87</span>
<span class="yarn-cmd">&lt;&lt;card honey&gt;&gt;</span>
<span class="yarn-line">Tutti i miei prodotti provengono dai miei alveari!</span> <span class="yarn-meta">#line:0aa9ce7</span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#shadow:what_to_buy </span>
<span class="yarn-line">Miele</span> <span class="yarn-meta">#line:honey</span>
   <span class="yarn-cmd">&lt;&lt;jump beekeper_pay_activity&gt;&gt;</span>
<span class="yarn-line">Cioccolato</span> <span class="yarn-meta">#line:chocolate</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:bread</span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Latte</span> <span class="yarn-meta">#line:milk</span>
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
<span class="yarn-line">Scegli le monete giuste per pagare.</span> <span class="yarn-meta">#shadow:select_money</span>
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
<span class="yarn-line">Ho messo i tuoi oggetti sul tavolo. Grazie!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Buona giornata!</span> <span class="yarn-meta">#shadow:nice_day</span>
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
&lt;&lt;if GetActivityResult("money_cheesemonger") &gt; 0&gt;&gt;
<span class="yarn-line">   Hai già acquistato da me!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Vuoi giocare di nuovo?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   SÌ</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NO</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_bonjour&gt;&gt;</span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
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
<span class="yarn-line">Ciao! Vendo formaggio e burro. Sono un casaro.</span> <span class="yarn-meta">#line:09eb222 </span>
<span class="yarn-line">Io uso sia latte vaccino che latte di capra.</span> <span class="yarn-meta">#line:02f4bc9 </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#shadow:what_to_buy </span>
<span class="yarn-line">Burro</span> <span class="yarn-meta">#line:butter </span>
   <span class="yarn-cmd">&lt;&lt;jump cheesemonger_pay_activity&gt;&gt;</span>
<span class="yarn-line">Olio</span> <span class="yarn-meta">#line:057f694 </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:087919f </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pomodori</span> <span class="yarn-meta">#line:067bfab </span>
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
<span class="yarn-line">Scegli le monete giuste per pagare.</span> <span class="yarn-meta">#shadow:select_money</span>
<span class="yarn-cmd">&lt;&lt;activity money_cheesemonger cheesemonger_payment_done&gt;&gt;</span>

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
<span class="yarn-line">Ho messo i tuoi oggetti sul tavolo. Grazie!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Buona giornata!</span> <span class="yarn-meta">#shadow:nice_day</span>
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
<span class="yarn-line">   Hai già acquistato da me!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Vuoi giocare di nuovo?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   SÌ</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NO</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_bonjour&gt;&gt;</span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
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
<span class="yarn-line">Ciao! Vendo UOVA. Sono un venditore ambulante di uova.</span> <span class="yarn-meta">#line:09a9960 </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#shadow:what_to_buy</span>
<span class="yarn-line">Uova</span> <span class="yarn-meta">#line:eggs</span>
   <span class="yarn-cmd">&lt;&lt;jump eggvendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Olio</span> <span class="yarn-meta">#line:06cc62e </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Pane</span> <span class="yarn-meta">#line:059920e </span>
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
<span class="yarn-line">Scegli le monete giuste per pagare.</span> <span class="yarn-meta">#shadow:select_money</span>
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
<span class="yarn-line">Ho messo i tuoi oggetti sul tavolo. Grazie!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Buona giornata!</span> <span class="yarn-meta">#shadow:nice_day</span>
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
<span class="yarn-line">   Hai già acquistato da me!</span> <span class="yarn-meta">#shadow:already_bought</span>
<span class="yarn-line">   Vuoi giocare di nuovo?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">   SÌ</span> <span class="yarn-meta">#shadow:yes</span>
     <span class="yarn-cmd">&lt;&lt;activity hard_money_zloty hard_payment_done&gt;&gt;</span>
<span class="yarn-line">   NO</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_understand&gt;&gt;</span>
<span class="yarn-line">Buongiorno!</span> <span class="yarn-meta">#shadow:hello </span>
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_bonjour&gt;&gt;</span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
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
<span class="yarn-line">Ciao! Vendo spezie. Sono un venditore ambulante.</span> <span class="yarn-meta">#line:0f83873 </span>
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
<span class="yarn-line">Cosa vuoi acquistare?</span> <span class="yarn-meta">#shadow:what_to_buy</span>
<span class="yarn-line">Cannella e zenzero</span> <span class="yarn-meta">#line:0fe40e7 </span>
   <span class="yarn-cmd">&lt;&lt;jump spicevendor_pay_activity&gt;&gt;</span>
<span class="yarn-line">Burro</span> <span class="yarn-meta">#line:0fa399a </span>
   <span class="yarn-cmd">&lt;&lt;jump talk_dont_sell&gt;&gt;</span>
<span class="yarn-line">Miele</span> <span class="yarn-meta">#line:0cec0d0 </span>
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
<span class="yarn-line">Scegli le monete giuste per pagare.</span> <span class="yarn-meta">#shadow:select_money</span>
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
<span class="yarn-line">Ho messo i tuoi oggetti sul tavolo. Grazie!</span> <span class="yarn-meta">#shadow:0567082 </span>
<span class="yarn-line">Arrivederci!</span> <span class="yarn-meta">#shadow:goodbye</span>
<span class="yarn-line">Grazie!</span> <span class="yarn-meta">#shadow:thanks</span>
<span class="yarn-line">Buona giornata!</span> <span class="yarn-meta">#shadow:nice_day</span>
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
<span class="yarn-line">Mi dispiace, non capisco...</span> <span class="yarn-meta">#line:0f9044b </span>
<span class="yarn-line">Che cosa?</span> <span class="yarn-meta">#line:09682b7 </span>
<span class="yarn-line">Eh?</span> <span class="yarn-meta">#line:0c1b3e0 </span>

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
<span class="yarn-line">Mi dispiace, non lo vendo.</span> <span class="yarn-meta">#line:08700b0 </span>

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
<span class="yarn-line">Prova a parlare anche con gli altri venditori.</span> <span class="yarn-meta">#line:06ae965 </span>

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
<span class="yarn-line">Farina</span> <span class="yarn-meta">#line:08e101e </span>
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
<span class="yarn-line">Miele</span> <span class="yarn-meta">#line:0817d3c </span>
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
<span class="yarn-line">Zucchero</span> <span class="yarn-meta">#line:05ad31e </span>
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
<span class="yarn-line">Burro</span> <span class="yarn-meta">#line:0a8a8cc </span>
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
<span class="yarn-line">Uovo</span> <span class="yarn-meta">#line:00ab8e2 </span>
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
<span class="yarn-line">Cannella</span> <span class="yarn-meta">#line:0f00ddd </span>
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
<span class="yarn-line">Zenzero</span> <span class="yarn-meta">#line:08049d5 </span>
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
<span class="yarn-line">La porta è chiusa a chiave.</span> <span class="yarn-meta">#line:042ecfc </span>

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
<span class="yarn-line">Ciao. Cosa vuoi?</span> <span class="yarn-meta">#line:0f11caf </span>
<span class="yarn-line">Voglio entrare nel Vecchio Municipio.</span> <span class="yarn-meta">#line:0449db3 </span>
   <span class="yarn-cmd">&lt;&lt;if $gingerbread_done == true&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;trigger open_door_castle&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;area area_castle&gt;&gt;</span>
<span class="yarn-line">      Prego, entrate! C'è qualcuno che vi aspetta.</span> <span class="yarn-meta">#line:0716b5d </span>
      <span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>
      <span class="yarn-cmd">&lt;&lt;SetInteractable door_castle false&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">      Il vecchio municipio è chiuso ai visitatori.</span> <span class="yarn-meta">#line:096470a </span>
   <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Mi sto solo guardando intorno.</span> <span class="yarn-meta">#line:09a9858 </span>
<span class="yarn-line">   Bene, buona giornata.</span> <span class="yarn-meta">#line:09ee9bf </span>

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
<span class="yarn-line">    Grazie per aver riordinato! Ora possiamo dare inizio alla Festa dei Pierogi!</span> <span class="yarn-meta">#line:07e852b </span>
    <span class="yarn-cmd">&lt;&lt;card pierogi&gt;&gt;</span>
<span class="yarn-line">    Hai mai provato i PIEROGI?</span> <span class="yarn-meta">#line:0da391b </span>
<span class="yarn-line">    SÌ</span> <span class="yarn-meta">#line:08ac4ff </span>
<span class="yarn-line">      È delizioso. Non è vero?</span> <span class="yarn-meta">#line:040912f </span>
<span class="yarn-line">    NO</span> <span class="yarn-meta">#line:0cb270e </span>
<span class="yarn-line">      Si tratta di uno speciale gnocco ripieno di ingredienti deliziosi, come formaggio o patate.</span> <span class="yarn-meta">#line:09da259 </span>
    <span class="yarn-cmd">&lt;&lt;card_hide&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_tower&gt;&gt;</span>
<span class="yarn-line">    Ho visto quel grosso cane blu salire le scale!</span> <span class="yarn-meta">#line:0ee5d9b </span>
<span class="yarn-line">    Vai a trovare Antura!</span> <span class="yarn-meta">#line:09877a9 </span>
<span class="yarn-line">    Ho acceso l'ascensore per te.</span> <span class="yarn-meta">#line:0f5e348 </span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;SetInteractable tower_lever true&gt;&gt;</span>
   <span class="yarn-cmd">&lt;&lt;task_start climb_the_tower antura_tower&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Ciao. Vogliamo aprire il Pierogi Festival</span> <span class="yarn-meta">#line:09c418a </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_trash&gt;&gt;</span>
<span class="yarn-line">    Ma quel grosso cane blu ha combinato un pasticcio nel corridoio.</span> <span class="yarn-meta">#line:067cefa </span>
<span class="yarn-line">    Puoi aiutarci a pulire la sala?</span> <span class="yarn-meta">#line:003385d  </span>
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
<span class="yarn-line">Ben fatto! Torna dallo chef.</span> <span class="yarn-meta">#line:08e9b5f </span>
<span class="yarn-cmd">&lt;&lt;target npc_pierogi&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-antura-tower"></a>

## antura_tower

<div class="yarn-node" data-title="antura_tower">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato Antura!</span> <span class="yarn-meta">#line:0307b40 </span>
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
<span class="yarn-line">Mi piace il PAN DI ZENZERO dolce</span> <span class="yarn-meta">#line:087d4b0 </span>
<span class="yarn-line">Mi piace il VECCHIO MERCATO!</span> <span class="yarn-meta">#line:0090fb8 </span>

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
<span class="yarn-line">La Old HALL ha un aspetto magnifico.</span> <span class="yarn-meta">#line:041418b </span>
<span class="yarn-line">Oggi assaggerò i PIEROGI.</span> <span class="yarn-meta">#line:068a9a7</span>
<span class="yarn-line">Torun è davvero bellissima!</span> <span class="yarn-meta">#line:08bc7dd </span>
<span class="yarn-line">Il mercato è così vivace!</span> <span class="yarn-meta">#line:0d92388 </span>

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
<span class="yarn-line">Ho bisogno di comprare degli ingredienti freschi.</span> <span class="yarn-meta">#line:0baa74d </span>
<span class="yarn-line">Il mercato ha i prodotti migliori.</span> <span class="yarn-meta">#line:042c6f0 </span>
<span class="yarn-line">Il pesce fresco è il migliore!</span> <span class="yarn-meta">#line:01269c6 </span>
<span class="yarn-line">Non vedo l'ora di cucinare un pasto delizioso!</span> <span class="yarn-meta">#line:0fc5cd3 </span>

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
<span class="yarn-line">Uno zloty equivale a 100 groszy.</span> <span class="yarn-meta">#line:0e6c526 #card:currency_zloty</span>
<span class="yarn-line">Gli zloty hanno dimensioni diverse per ogni valore.</span> <span class="yarn-meta">#line:021819a #card:currency_zloty</span>

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
<span class="yarn-line">Un fornaio cuoce il pane.</span> <span class="yarn-meta">#line:0606dc5 #card:person_baker</span>
<span class="yarn-line">Un venditore di spezie vende spezie ed erbe aromatiche.</span> <span class="yarn-meta">#line:0ca5a9b #card:person_spicevendor</span>
<span class="yarn-line">Un casaro vende formaggio e latte.</span> <span class="yarn-meta">#line:0b5b1c5 #card:person_cheesemonger</span>
<span class="yarn-line">Un fruttivendolo vende frutta e verdura.</span> <span class="yarn-meta">#line:03b9a2e #card:person_greengrocer</span>

</code>
</pre>
</div>


