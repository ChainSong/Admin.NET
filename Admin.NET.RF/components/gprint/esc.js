/**
 * ESC/POS 打印指令封装
 * 用于生成佳博打印机等热敏打印机的打印指令
 */

class ESCPrinter {
	constructor() {
		this.buffer = [];
	}

	/**
	 * 初始化打印机
	 */
	init() {
		this.buffer.push(0x1B, 0x40);
		return this;
	}

	/**
	 * 设置对齐方式
	 * @param {Number} align 0:左对齐 1:居中 2:右对齐
	 */
	setAlign(align) {
		this.buffer.push(0x1B, 0x61, align);
		return this;
	}

	/**
	 * 设置字体大小
	 * @param {Number} width 倍宽 1-8
	 * @param {Number} height 倍高 1-8
	 */
	setSize(width, height) {
		this.buffer.push(0x1D, 0x21, (height - 1) << 4 | (width - 1));
		return this;
	}

	/**
	 * 设置加粗
	 * @param {Boolean} bold true:加粗 false:取消加粗
	 */
	setBold(bold) {
		this.buffer.push(0x1B, 0x45, bold ? 1 : 0);
		return this;
	}

	/**
	 * 设置下划线
	 * @param {Number} underline 0:无下划线 1:单点下划线 2:双点下划线
	 */
	setUnderline(underline) {
		this.buffer.push(0x1B, 0x2D, underline);
		return this;
	}

	/**
	 * 打印并换行
	 */
	setPrint() {
		this.buffer.push(0x0A);
		return this;
	}

	/**
	 * 打印并走纸n行
	 * @param {Number} lines 行数
	 */
	setPrintAndFeedLine(lines) {
		this.buffer.push(0x1B, 0x64, lines);
		return this;
	}

	/**
	 * 打印文本
	 * @param {String} text 文本内容
	 */
	setText(text) {
		if (text) {
			const encoder = new TextEncoder('gb18030');
			const bytes = encoder.encode(text);
			for (let i = 0; i < bytes.length; i++) {
				this.buffer.push(bytes[i]);
			}
		}
		return this;
	}

	/**
	 * 打印条码
	 * @param {Number} x x坐标
	 * @param {Number} y y坐标
	 * @param {Number} type 条码类型
	 * @param {Number} width 条码宽度
	 * @param {Number} height 条码高度
	 * @param {String} data 条码数据
	 */
	setBar(x, y, type, width, height, data) {
		// 设置条码位置（如果是标签打印机）
		this.buffer.push(0x1D, 0x48, x, y);
		// 设置条码类型
		this.buffer.push(0x1D, 0x6B, type);
		// 设置条码宽度
		this.buffer.push(0x1D, 0x77, width);
		// 设置条码高度
		this.buffer.push(0x1D, 0x68, height);
		// 打印条码数据
		if (data) {
			const encoder = new TextEncoder('gb18030');
			const bytes = encoder.encode(data);
			for (let i = 0; i < bytes.length; i++) {
				this.buffer.push(bytes[i]);
			}
		}
		// 条码结束符
		this.buffer.push(0x00);
		return this;
	}

	/**
	 * 打印二维码
	 * @param {Number} x x坐标
	 * @param {Number} y y坐标
	 * @param {String} level 纠错级别 L/M/Q/H
	 * @param {Number} version 版本 1-40
	 * @param {String} mode 编码模式 A/M/N
	 * @param {String} data 二维码数据
	 */
	setQR(x, y, level, version, mode, data) {
		// 设置二维码位置（如果是标签打印机）
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, x & 0xFF, (x >> 8) & 0xFF);
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, y & 0xFF, (y >> 8) & 0xFF);

		// 设置纠错级别
		const levelMap = { 'L': 0x31, 'M': 0x32, 'Q': 0x33, 'H': 0x34 };
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, levelMap[level] || 0x31);

		// 设置版本
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, version & 0xFF);

		// 设置编码模式
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, mode.charCodeAt(0));

		// 设置二维码数据长度
		const encoder = new TextEncoder('gb18030');
		const bytes = encoder.encode(data);
		const len = bytes.length + 3;
		this.buffer.push(0x1D, 0x28, 0x6B, len & 0xFF, (len >> 8) & 0xFF, 0x31, 0x50, 0x30);

		// 打印二维码数据
		for (let i = 0; i < bytes.length; i++) {
			this.buffer.push(bytes[i]);
		}

		// 打印二维码
		this.buffer.push(0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x51);

		return this;
	}

	/**
	 * 设置纸张间距
	 * @param {Number} gap 间距值
	 */
	setGap(gap) {
		this.buffer.push(0x1D, 0x28, 0x66, 0x02, 0x00, gap & 0xFF, (gap >> 8) & 0xFF);
		return this;
	}

	/**
	 * 获取打印数据
	 */
	getData() {
		return new Uint8Array(this.buffer);
	}
}

export default {
	createNew: function() {
		return new ESCPrinter();
	}
};
