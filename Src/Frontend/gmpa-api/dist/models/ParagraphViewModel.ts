/* tslint:disable */
/* eslint-disable */
/**
 * GMPA - API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
/**
 * 
 * @export
 * @interface ParagraphViewModel
 */
export interface ParagraphViewModel {
    /**
     * 
     * @type {string}
     * @memberof ParagraphViewModel
     */
    alias?: string | null;
    /**
     * 
     * @type {object}
     * @memberof ParagraphViewModel
     */
    text?: object;
}

/**
 * Check if a given object implements the ParagraphViewModel interface.
 */
export function instanceOfParagraphViewModel(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function ParagraphViewModelFromJSON(json: any): ParagraphViewModel {
    return ParagraphViewModelFromJSONTyped(json, false);
}

export function ParagraphViewModelFromJSONTyped(json: any, ignoreDiscriminator: boolean): ParagraphViewModel {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'alias': !exists(json, 'alias') ? undefined : json['alias'],
        'text': !exists(json, 'text') ? undefined : json['text'],
    };
}

export function ParagraphViewModelToJSON(value?: ParagraphViewModel | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'alias': value.alias,
        'text': value.text,
    };
}
