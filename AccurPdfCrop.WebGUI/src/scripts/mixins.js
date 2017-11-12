export const sharedMixin = {
	created() {
		if (this.$options.shared) {
			const name = this.$options.name;
			let shRoot;
			if (! this.$root._$_vueShared)
				this.$root._$_vueShared = {};
			shRoot = this.$root._$_vueShared;
			if (! shRoot[name]) {
				shRoot[name] = {
					counter: 0,
					ref: this.$options.shared
				};
				if (typeof shRoot[name].ref === 'function')
					shRoot[name].ref = shRoot[name].ref();
				if (this.$options.sharedCreated)
					this.$options.sharedCreated(shRoot[name].ref);
			}
			++shRoot[name].counter;
			this.$shared = shRoot[name].ref;
		}
	},
	destroyed() {
		const name = this.$options.name;
		const shRoot = this.$root._$_vueShared;
		if (shRoot[name].counter <= 1) {
			if (this.$options.sharedBeforeDestroy)
				this.$options.sharedBeforeDestroy(shRoot[name].ref);
			delete shRoot[name];
		}
		else
			--shRoot[name].counter;
	}
} 
