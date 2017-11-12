// TODO: split config on debug / production


const path = require('path');
const fs = require('fs');
const webpack = require('webpack');

const ExtractTextPlugin = require('extract-text-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
	entry() {
		const PATH_TO_ENTRIES = './src/entries';
		const PATH_TO_COMP_ENTRIES = './src/components';

		// returns paths relative to rootPath of all js-files in dir (including subdirs)
		function getFiles(rootPath) {
			const results = [];

			if (!fs.existsSync(rootPath)){
				console.log('entry dir not found:', rootPath);
				return;
			}

			function addFilesToResult(relPath) {
				const absPath = path.join(rootPath, relPath);
				const entries = fs.readdirSync(absPath);
				entries.forEach((entry) => {
					const fullName = path.join(absPath, entry);
					const stat = fs.lstatSync(fullName);

					if (stat.isDirectory()){
						addFilesToResult(path.join(relPath, entry));
					}
					else {
						if (entry.endsWith('.js'))
							results.push(path.join(relPath, entry));
					}
				});
			}

			addFilesToResult("");

			return results;
		}

		const entryPoints = {};
		getFiles(PATH_TO_ENTRIES).forEach((relFileName) => {
			entryPoints[relFileName.substring(0, relFileName.length - 3)] = './' + path.join(PATH_TO_ENTRIES, relFileName);
		});
		getFiles(PATH_TO_COMP_ENTRIES).forEach((relFileName) => {
			entryPoints[relFileName.substring(0, relFileName.length - 3)] = './' + path.join(PATH_TO_COMP_ENTRIES, relFileName);
		});

		entryPoints['vue-runtime'] = 'vue/dist/vue.runtime.js';

		return entryPoints;
	},
	output: {
		filename: './dist/js/[name].js' 
	},
	//noParse: 
	module: {
		noParse(context) {
			if(/vue.runtime.js/.test(context)) {
				//console.log('not parsed: ' + context);
				return true;
			}
		},
		rules: [
			{
				test: /\.vue$/,
				loader: 'vue-loader',
				options: {
				    extractCSS: true
				}
			},
			{
				test: /\.css$/,
				use: ExtractTextPlugin.extract({
					use: 'css-loader'
				})
			},
			{
				test: /\.js$/,
				loader: 'babel-loader',
				exclude: /node_modules/
			}
		]
	},
	plugins: [
		new ExtractTextPlugin('./dist/css/[name].css'),
		new webpack.optimize.CommonsChunkPlugin({
			name: 'vue-runtime',
			minChunks: Infinity
		}),
		new webpack.optimize.CommonsChunkPlugin({
			name: 'manifest'
		}),
		new webpack.optimize.ModuleConcatenationPlugin(),
		// new BundleAnalyzerPlugin({
		// 	analyzerMode: 'static',
		// 	reportFilename: './temp/bundle-analyze-report.html'
		// }),
		new CopyWebpackPlugin([{ 
			from: './src/index.html', 
			to: './dist/index.html' 
		}])
	],
	//devtool: 'eval-source-map'
}