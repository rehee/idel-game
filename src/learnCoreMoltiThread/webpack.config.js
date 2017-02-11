var path = require('path');
var webpack = require('webpack');
var commonsPlugin = new webpack.optimize.CommonsChunkPlugin('shared.js');
module.exports = {
	entry: {
		app: './wwwroot/App/app.js'
	},
	output: {
		// path: ,
		// publicPath: '/',
		path: path.resolve('wwwroot/src/js/'),
		filename: '[name].js'
	},
	plugins:[commonsPlugin],
	module: {
		loaders: [
			{
				test: /\.js$/,
				exclude: /node_modules/,
				loader: 'babel-loader'
			},
			{
				test:/\.css$/,
				exclude:/node_modules/,
				loader: "style-loader!css-loader"
			}
						//,
			//{
			//		test: /\.scss$/,
			//		exclude: /node_modules/,
			//		loader: "style-loader!css-loader!sass-loader"
			//}
		]
	},
	resolve: {
		extensions: ['', '.js']
	},
	watch: true
};
