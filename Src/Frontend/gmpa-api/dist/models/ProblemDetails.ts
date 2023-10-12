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
 * @interface ProblemDetails
 */
export interface ProblemDetails {
    [key: string]: any | any;
    /**
     * 
     * @type {string}
     * @memberof ProblemDetails
     */
    type?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ProblemDetails
     */
    title?: string | null;
    /**
     * 
     * @type {number}
     * @memberof ProblemDetails
     */
    status?: number | null;
    /**
     * 
     * @type {string}
     * @memberof ProblemDetails
     */
    detail?: string | null;
    /**
     * 
     * @type {string}
     * @memberof ProblemDetails
     */
    instance?: string | null;
}

/**
 * Check if a given object implements the ProblemDetails interface.
 */
export function instanceOfProblemDetails(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function ProblemDetailsFromJSON(json: any): ProblemDetails {
    return ProblemDetailsFromJSONTyped(json, false);
}

export function ProblemDetailsFromJSONTyped(json: any, ignoreDiscriminator: boolean): ProblemDetails {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
            ...json,
        'type': !exists(json, 'type') ? undefined : json['type'],
        'title': !exists(json, 'title') ? undefined : json['title'],
        'status': !exists(json, 'status') ? undefined : json['status'],
        'detail': !exists(json, 'detail') ? undefined : json['detail'],
        'instance': !exists(json, 'instance') ? undefined : json['instance'],
    };
}

export function ProblemDetailsToJSON(value?: ProblemDetails | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
            ...value,
        'type': value.type,
        'title': value.title,
        'status': value.status,
        'detail': value.detail,
        'instance': value.instance,
    };
}

