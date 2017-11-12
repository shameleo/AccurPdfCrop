<template>
	<div style="background: white; border: 1px solid gray; height: 500px; width: 400px; position: relative;">
		<selection-area :x1="0" :x2="200" :y1="0" :y2="300" />
	</div>
</template>


<script>
	import SelectionArea from './SelectionArea.vue'

	export default {
		components: { SelectionArea }
	}
</script>