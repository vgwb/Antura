---
title: Pierniki i targ spożywczy (pl_06) - Script
hide:
---

# Pierniki i targ spożywczy (pl_06) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-06-gingerbread-food-market/37/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1211829352#gid=1211829352)  
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
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - ingredient collection</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - money activities to pay</span>
<span class="yarn-header-dim">// - collect ingredients (eggs, flour, milk, butter, honey, cloves, cinnamon, ginger)</span>
<span class="yarn-header-dim">// - buy pierogi for the cook</span>
<span class="yarn-header-dim">// - bake gingerbread (order/memory of ingredients)</span>
<span class="yarn-header-dim">// - Pierogi Challenge: order/memory of ingredients (flour, eggs, cheese, potatoes)</span>
<span class="yarn-header-dim">// Words used: Toruń, market, vendor, grocer, beekeeper, dairy, eggs, milk, butter, flour, honey, cloves, cinnamon, ginger, pierogi, molds, coins, zloty, kitchen, gingerbread, medieval</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ingredients = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $needed_ingredients = 8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $pierogi_bought = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_eggs = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_flour = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_milk = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_butter = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_honey = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cloves = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_cinnamon = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_ginger = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card medieval_market&gt;&gt;</span>
<span class="yarn-line">Witamy w TORUNIU!</span> <span class="yarn-meta">#line:080555e </span>

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
<span class="yarn-line">Świetna robota w MARKET.</span> <span class="yarn-meta">#line:073978d </span>
<span class="yarn-line">Kupiłeś i upiekłeś.</span> <span class="yarn-meta">#line:023d1f0 </span>
<span class="yarn-line">Gotowy na dodatkowe zadanie?</span> <span class="yarn-meta">#line:04e4583 </span>
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
<span class="yarn-line">Narysuj stoisko na targu.</span> <span class="yarn-meta">#line:01f8217 </span>
<span class="yarn-line">Dodaj JAJA, MĄKĘ, MLEKO, MASŁO.</span> <span class="yarn-meta">#line:0435fc9 </span>
<span class="yarn-line">Dodaj MIÓD, GOŹDZIKI, CYNAMON I IMBIR.</span> <span class="yarn-meta">#line:0fed740 </span>
<span class="yarn-line">Podaj 2 ceny w zł.</span> <span class="yarn-meta">#line:084419e </span>
<span class="yarn-line">Pokaż to znajomemu.</span> <span class="yarn-meta">#line:0f16df6 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro"></a>

## intro

<div class="yarn-node" data-title="intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Antura jest wolna, ale głodna.</span> <span class="yarn-meta">#line:0382fdb </span>
<span class="yarn-cmd">&lt;&lt;card torun_town_hall&gt;&gt;</span>
<span class="yarn-line">Jedziemy na toruński rynek.</span> <span class="yarn-meta">#line:03e11fa </span>
<span class="yarn-line">Dostaniemy jedzenie i składniki.</span> <span class="yarn-meta">#line:0522f6a </span>


</code>
</pre>
</div>

<a id="ys-node-market-hub"></a>

## market_hub

<div class="yarn-node" data-title="market_hub">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card butcher&gt;&gt;</span>
<span class="yarn-line">Odwiedź stoiska i porozmawiaj ze sprzedawcami.</span> <span class="yarn-meta">#line:0c470ae </span>
<span class="yarn-line">Sprzedawca (przyprawy)</span> <span class="yarn-meta">#line:0c33932 </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_grocer&gt;&gt;</span>
<span class="yarn-line">Pszczelarz (miód)</span> <span class="yarn-meta">#line:0431cbc </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_beekeeper&gt;&gt;</span>
<span class="yarn-line">Sprzedawca nabiału (mleko, masło)</span> <span class="yarn-meta">#line:0bfeb9e </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_dairy&gt;&gt;</span>
<span class="yarn-line">Sprzedawca jajek (jajka)</span> <span class="yarn-meta">#line:0aed436 </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_eggs&gt;&gt;</span>
<span class="yarn-line">Sprzedawca warzyw (owoce, warzywa)</span> <span class="yarn-meta">#line:0b3cc4d </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_greengrocer&gt;&gt;</span>
<span class="yarn-line">Cook (pierogi)</span> <span class="yarn-meta">#line:0486ae8 </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus shop_cook&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-grocer"></a>

## shop_grocer

<div class="yarn-node" data-title="shop_grocer">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sprzedaję GOŹDZIKI, CYNAMON, IMBIR. Jestem sprzedawcą spożywczym.</span> <span class="yarn-meta">#line:0a66f1e </span>
<span class="yarn-line">Kup goździki (1zł)</span> <span class="yarn-meta">#line:0eff39a </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_cloves&gt;&gt;</span>
<span class="yarn-line">Kup cynamon (1zł)</span> <span class="yarn-meta">#line:02f52e1 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_cinnamon&gt;&gt;</span>
<span class="yarn-line">Kup imbir (1zł)</span> <span class="yarn-meta">#line:077537d </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_ginger&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-beekeeper"></a>

## shop_beekeeper

<div class="yarn-node" data-title="shop_beekeeper">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card beekeeper&gt;&gt;</span>
<span class="yarn-line">Sprzedaję MIÓD. Jestem pszczelarzem.</span> <span class="yarn-meta">#line:001922d </span>
<span class="yarn-line">Kup miód (1zł)</span> <span class="yarn-meta">#line:086cd0b </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_honey&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-dairy"></a>

## shop_dairy

<div class="yarn-node" data-title="shop_dairy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card dairy_vendor&gt;&gt;</span>
<span class="yarn-line">Sprzedaję MLEKO i MASŁO. Jestem sprzedawcą nabiału.</span> <span class="yarn-meta">#line:0acb509 </span>
<span class="yarn-line">Kup mleko (5zł)</span> <span class="yarn-meta">#line:0cd7285 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_milk&gt;&gt;</span>
<span class="yarn-line">Kup masło (5zł)</span> <span class="yarn-meta">#line:0e1775b </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_butter&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-eggs"></a>

## shop_eggs

<div class="yarn-node" data-title="shop_eggs">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card egg_vendor&gt;&gt;</span>
<span class="yarn-line">Sprzedaję JAJKA. Jestem sprzedawcą jajek.</span> <span class="yarn-meta">#line:03de236 </span>
<span class="yarn-line">Kup jajka (10zł)</span> <span class="yarn-meta">#line:07a79e8 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_eggs&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-greengrocer"></a>

## shop_greengrocer

<div class="yarn-node" data-title="shop_greengrocer">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Sprzedaję OWOCE i WARZYWA. Jestem warzywniakiem.</span> <span class="yarn-meta">#line:082b55f </span>
<span class="yarn-line">Dzisiaj będziesz potrzebować PRZYPRAW i produktów do pieczenia.</span> <span class="yarn-meta">#line:0af9f1e </span>
<span class="yarn-line">Kup mąkę (2zł)</span> <span class="yarn-meta">#line:0cf3d86 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-shop-cook"></a>

## shop_cook

<div class="yarn-node" data-title="shop_cook">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card cook&gt;&gt;</span>
<span class="yarn-line">Sprzedaję PIEROGI.</span> <span class="yarn-meta">#line:0d24718 </span>
<span class="yarn-cmd">&lt;&lt;card pierogi&gt;&gt;</span>
<span class="yarn-line">Kup PIEROGI i korzystaj z mojej KUCHNI.</span> <span class="yarn-meta">#line:0eda999 </span>
<span class="yarn-line">Kup pierogi (20zł)</span> <span class="yarn-meta">#line:01729c1 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_pierogi&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pay-cloves"></a>

## pay_cloves

<div class="yarn-node" data-title="pay_cloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card currency_zloty&gt;&gt;</span>
<span class="yarn-line">Wybierz monety, którymi zapłacisz 1 zł.</span> <span class="yarn-meta">#line:019a160 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cloves_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-cloves-done"></a>

## add_cloves_done

<div class="yarn-node" data-title="add_cloves_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_cloves&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_cloves = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-cinnamon"></a>

## pay_cinnamon

<div class="yarn-node" data-title="pay_cinnamon">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coins&gt;&gt;</span>
<span class="yarn-line">Wybierz monety, którymi zapłacisz 1 zł.</span> <span class="yarn-meta">#line:055af31 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_cinnamon_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-cinnamon-done"></a>

## add_cinnamon_done

<div class="yarn-node" data-title="add_cinnamon_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_cinnamon&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_cinnamon = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-ginger"></a>

## pay_ginger

<div class="yarn-node" data-title="pay_ginger">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, którymi zapłacisz 1 zł.</span> <span class="yarn-meta">#line:0172345 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_ginger_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-ginger-done"></a>

## add_ginger_done

<div class="yarn-node" data-title="add_ginger_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_ginger&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_ginger = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-honey"></a>

## pay_honey

<div class="yarn-node" data-title="pay_honey">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, którymi zapłacisz 1 zł.</span> <span class="yarn-meta">#line:0c1dcdb </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_1 add_honey_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-honey-done"></a>

## add_honey_done

<div class="yarn-node" data-title="add_honey_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_honey&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_honey = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pay-milk"></a>

## pay_milk

<div class="yarn-node" data-title="pay_milk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, aby zapłacić 5 zł.</span> <span class="yarn-meta">#line:04a5053 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_milk_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-milk-done"></a>

## add_milk_done

<div class="yarn-node" data-title="add_milk_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_milk&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_milk = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-butter"></a>

## pay_butter

<div class="yarn-node" data-title="pay_butter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, aby zapłacić 5 zł.</span> <span class="yarn-meta">#line:0bd010e </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_5 add_butter_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-butter-done"></a>

## add_butter_done

<div class="yarn-node" data-title="add_butter_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_butter&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_butter = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-eggs"></a>

## pay_eggs

<div class="yarn-node" data-title="pay_eggs">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, aby zapłacić 10 zł.</span> <span class="yarn-meta">#line:0bdf451 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_10 add_eggs_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-eggs-done"></a>

## add_eggs_done

<div class="yarn-node" data-title="add_eggs_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_eggs&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_eggs = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-flour"></a>

## pay_flour

<div class="yarn-node" data-title="pay_flour">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wybierz monety, aby zapłacić 2 zł.</span> <span class="yarn-meta">#line:0aac231 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_2 add_flour_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-add-flour-done"></a>

## add_flour_done

<div class="yarn-node" data-title="add_flour_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$got_flour&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $ingredients = $ingredients + 1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $got_flour = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-pay-pierogi"></a>

## pay_pierogi

<div class="yarn-node" data-title="pay_pierogi">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: pay</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MONEY_20 pierogi_done&gt;&gt;</span>
<span class="yarn-line">Wybierz monety, aby zapłacić 20 zł.</span> <span class="yarn-meta">#line:0dd43a0 </span>

</code>
</pre>
</div>

<a id="ys-node-pierogi-done"></a>

## pierogi_done

<div class="yarn-node" data-title="pierogi_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: type=Variables</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $pierogi_bought = true&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-flour-hint"></a>

## flour_hint

<div class="yarn-node" data-title="flour_hint">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: market</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">MĄKĘ możesz kupić tutaj.</span> <span class="yarn-meta">#line:077f3e2 </span>
<span class="yarn-line">Kup mąkę (2zł)</span> <span class="yarn-meta">#line:00c5178 </span>
	<span class="yarn-cmd">&lt;&lt;jump pay_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-go-to-kitchen"></a>

## go_to_kitchen

<div class="yarn-node" data-title="go_to_kitchen">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetnie! Masz wszystkie składniki i PIEROGI.</span> <span class="yarn-meta">#line:0dda4ac </span>
<span class="yarn-cmd">&lt;&lt;card kitchen&gt;&gt;</span>
<span class="yarn-line">Możesz korzystać z mojej KUCHNI.</span> <span class="yarn-meta">#line:019c938 </span>
<span class="yarn-line">Upieczmy TORUŃSKIE PIERNIKI!</span> <span class="yarn-meta">#line:0b5d503 </span>
<span class="yarn-cmd">&lt;&lt;jump bake_gingerbread&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-bake-gingerbread"></a>

## bake_gingerbread

<div class="yarn-node" data-title="bake_gingerbread">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread_mold&gt;&gt;</span>
<span class="yarn-line">Dopasuj części PIERNIKA w kolejności.</span> <span class="yarn-meta">#line:0e54683 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER GINGERBREAD_ORDER_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-gingerbread-order-done"></a>

## GINGERBREAD_ORDER_DONE

<div class="yarn-node" data-title="GINGERBREAD_ORDER_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gingerbread&gt;&gt;</span>
<span class="yarn-line">Dobra robota! PIERNIK jest gotowy.</span> <span class="yarn-meta">#line:0f082d6 </span>
<span class="yarn-line">Teraz wyzwanie PIEROGI!</span> <span class="yarn-meta">#line:0c0dc81 </span>
<span class="yarn-cmd">&lt;&lt;jump pierogi_challenge&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pierogi-challenge"></a>

## pierogi_challenge

<div class="yarn-node" data-title="pierogi_challenge">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Kroki zamówienia: MĄKA JAJA SER ZIEMNIAKI.</span> <span class="yarn-meta">#line:01a6d63 </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER PIEROGI_ORDER_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-pierogi-order-done"></a>

## PIEROGI_ORDER_DONE

<div class="yarn-node" data-title="PIEROGI_ORDER_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: kitchen</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dziękuję! To jest toruńskie jedzenie.</span> <span class="yarn-meta">#line:07b1ab4 </span>
<span class="yarn-line">Teraz jestem szczęśliwy i pełny.</span> <span class="yarn-meta">#line:0f8e153 </span>
<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-intro"></a>

## assessment_intro

<div class="yarn-node" data-title="assessment_intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Assessment</span>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Czas na szybką kontrolę.</span> <span class="yarn-meta">#line:0bad7e0 </span>
<span class="yarn-cmd">&lt;&lt;jump assessment_vocab&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-vocab"></a>

## assessment_vocab

<div class="yarn-node" data-title="assessment_vocab">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">tags: activity</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dopasuj słowa do obrazków</span> <span class="yarn-meta">#line:04fee1b </span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_ORDER VOCAB_MATCH_DONE&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-vocab-match-done"></a>

## VOCAB_MATCH_DONE

<div class="yarn-node" data-title="VOCAB_MATCH_DONE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: assessment</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świetna robota! Pomogłeś Anturze nauczyć się słów.</span> <span class="yarn-meta">#line:0cb84a7 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Lubię słodkie PIERNIKI.</span> <span class="yarn-meta">#line:087d4b0 </span>
<span class="yarn-line">Monety brzęczą w mojej torbie.</span> <span class="yarn-meta">#line:0090fb8 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-vendor"></a>

## spawned_vendor

<div class="yarn-node" data-title="spawned_vendor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Świeże JAJA i MLEKO dzisiaj.</span> <span class="yarn-meta">#line:039e272 </span>
<span class="yarn-line">Pierogi sprzedają się bardzo szybko.</span> <span class="yarn-meta">#line:07b9a99 </span>

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
<span class="yarn-line">Stary HALL wygląda tak wysoko.</span> <span class="yarn-meta">#line:041418b </span>
<span class="yarn-line">Spróbuję PIEROGÓW.</span> <span class="yarn-meta">#line:068a9a7 </span>

</code>
</pre>
</div>


