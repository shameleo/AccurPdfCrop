<template>
	<div class="selection" @mousedown="startMoving($event)" 
		 :style="{ top: t + 'px', left: l + 'px', height: height + 'px', width: width + 'px' }">
		<div class="corner top left" @mousedown="startMoving($event, 'l', 't')"></div>
		<div class="corner top right" @mousedown="startMoving($event, 'r', 't')"></div>
		<div class="corner bottom left" @mousedown="startMoving($event, 'l', 'b')"></div>
		<div class="corner bottom right" @mousedown="startMoving($event, 'r', 'b')"></div>
		<div class="side top" @mousedown="startMoving($event, null, 't')"></div>
		<div class="side bottom" @mousedown="startMoving($event, null, 'b')"></div>
		<div class="side left" @mousedown="startMoving($event, 'l', null)"></div>
		<div class="side right" @mousedown="startMoving($event, 'r', null)"></div>
	</div>
</template>


<script>
	import { sharedMixin } from '../../scripts/mixins.js'

	export default {
		mixins: [ sharedMixin ],
		shared() {
			const $shared = {
				currHandler: null,
				handlerOffsetX: null,
				handlerOffsetY: null,
				prevX: null,
				prevY: null,
				cursors: {
					lt: 'nwse-resize',
					rt: 'nesw-resize',
					lb: 'nesw-resize',
					rb: 'nwse-resize',
					l: 'ew-resize',
					r: 'ew-resize',
					t: 'ns-resize',
					b: 'ns-resize',
					all: 'move'
				},
				onMouseUp() {
					if ($shared.currHandler) {
						window.removeEventListener('mousemove', $shared.currHandler);
						$shared.currHandler = null;
						document.getElementsByTagName('body')[0].style.cursor = '';
					}
				}
			};

			return $shared;
		},
		sharedCreated($shared) {
			window.addEventListener('mouseup', $shared.onMouseUp);
		},
		sharedBeforeDestroy($shared) {
			window.removeEventListener('mouseup', $shared.onMouseUp);
		},
		props: ['x1', 'x2', 'y1', 'y2', 'applyExtLimits'],
		data(){
			return {
				t: this.x1,
				b: this.x2,
				l: this.y1,
				r: this.y2
			}	
		},
		methods: {
			startMoving(e, xKey, yKey){
				const $shared = this.$shared;
				let handlerKey;

				if ($shared.currHandler) {
					return;
				}

				if (xKey || yKey) {
					handlerKey = (xKey || '') + (yKey || '');
					$shared.currHandler = function(e){
						let delta = {
							x: xKey ? e.clientX - $shared.prevX : 0,
							y: yKey ? e.clientY - $shared.prevY : 0
						};

						let requestedDelta = {
							x: delta.x,
							y: delta.y
						};

						if (Math.sign(delta.x) == -Math.sign($shared.handlerOffsetX)) {
							if (Math.abs($shared.handlerOffsetX) < Math.abs(delta.x)){
								delta.x += $shared.handlerOffsetX;
							}
							else {
								delta.x = 0; 
							}
						}

						if(Math.sign(delta.y) == -Math.sign($shared.handlerOffsetY))
							if(Math.abs($shared.handlerOffsetY) < Math.abs(delta.y)){
								delta.y += $shared.handlerOffsetY;
							}
							else {
								delta.y = 0; 
							}

						delta = this.applyIntLimits(delta, xKey, yKey);

						if(this.applyExtLimits)
							delta = this.applyExtLimits(delta, xKey, yKey, currX, currY);	//надо еще текущие кооординаты передать

						$shared.handlerOffsetX += requestedDelta.x - delta.x;
						$shared.handlerOffsetY += requestedDelta.y - delta.y;

						this[xKey] += delta.x;
						this[yKey] += delta.y;
						$shared.prevX = e.clientX;
						$shared.prevY = e.clientY;
					}.bind(this);
				}
				else {
					handlerKey = 'all';
					$shared.currHandler = function(e){
						let delta = {
							x: e.clientX - $shared.prevX,
							y: e.clientY - $shared.prevY
						};

						if (this.applyExtLimits)
							delta = this.applyExtLimits(delta, null, null, currX, currY);

						this.l += delta.x;
						this.r += delta.x;
						this.t += delta.y;
						this.b += delta.y;
						$shared.prevX = e.clientX;
						$shared.prevY = e.clientY;
					}.bind(this)
				}

				$shared.handlerOffsetX = 0;
				$shared.handlerOffsetY = 0;
				$shared.prevX = e.clientX;
				$shared.prevY = e.clientY;
				document.getElementsByTagName('body')[0].style.cursor = $shared.cursors[handlerKey];
				window.addEventListener('mousemove', $shared.currHandler);

				e.preventDefault();
			},
			applyIntLimits(delta, xKey, yKey){
				const minWidth = 50, minHeight = 50;
				let	maxAbsDw = this.width - minWidth,
					maxAbsDh = this.height - minHeight
				
				if (xKey == 'l' && maxAbsDw < delta.x)
					delta.x = maxAbsDw;
				else if (xKey == 'r' && maxAbsDw < -delta.x)
					delta.x = -maxAbsDw;

				if (yKey == 't' && maxAbsDh < delta.y)
					delta.y = maxAbsDh;
				else if (yKey == 'b' && maxAbsDh < -delta.y)
					delta.y = -maxAbsDh;

				return delta;
			}
		},
		computed: {
			width: {
				get() { return this.r - this.l; },
				set(width) { this.r = this.l + width; }
			},
			height: {
				get() { return this.b - this.t; },
				set(height) { this.b = this.t + height; }
			}
		}
	}
</script>


<style scoped>
	.selection {
		border: 1px dashed gray;
		width: 400px;
		height: 400px;
		position: absolute;
	}

	.corner{
		width: 10px;
		height: 10px;
		background: gray;
		opacity: 0.3;
		position: absolute;
	}

	.corner.top {
		top: -1px;
	}

	.corner.bottom {
		bottom: -1px;
	}

	.corner.left {	
		left: -1px;
	}

	.corner.right {
		right: -1px;
	}

	.corner.top.left,
	.corner.bottom.right {
	  cursor: nwse-resize;
	}

	.corner.top.right,
	.corner.bottom.left {
		cursor: nesw-resize;
	}

	.side {
		position: absolute;
		background: transparent;
	}

	.side.top {
		top: 0;
	}

	.side.bottom {
		bottom: 0;
	}

	.side.left {
		left: 0;
	}

	.side.right {
		right: 0;
	}

	.side.top,
	.side.bottom {
		left: 10px;
		right: 10px;
		height: 5px;
		cursor: ns-resize;
	}

	.side.left,
	.side.right {
		top: 10px;
		bottom: 10px;
		width: 5px;
		cursor: ew-resize;
	}
</style> 
